using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorAge
{
    public class GetSql
    {
        static string _sql;
        static string _sqlExcel;
        /// <summary>
        /// 明细页面sql
        /// </summary>
        /// <param name="site">据点</param>
        /// <param name="type">cct 车贴
        ///                    cdt 单透
        ///                    cpp PPF
        ///                    cwg 外购
        ///                    
        ///                    bbj 背胶大卷
        ///                    bdt 单透大卷
        ///                    bgz 硅纸
        ///                    bsz 素纸
        ///                    
        ///                    yyz 原纸
        ///                    yhg 化工
        ///                    yjs 胶水
        ///                    ymw 膜 外购
        ///                    ymz 膜 自产
        /// </param>
        /// <param name="dayType">
        ///              a 1-15天
        ///              b 16-30天
        ///              c 31-90天
        ///              d 91-180天
        ///              e >180
        ///              f 其他
        /// </param>
        /// <returns></returns>
        public static string MXsql(string site,string type,string dayType,int pageIndex)
        {

            _sql = " SELECT ROWNUM rm,inagsite,inag004,inayl003,inag001,imaal003,imaal004,inag002,inaml004,  " +
                "           inag003,NVL(inag008,0)inag008,inag007,nvl(inag025,0) inag025,inag024,(TRUNC(SYSDATE) - tmb.tmdate) age " +
                "      FROM inag_t " +
                "     INNER JOIN imaa_t ON imaaent = inagent AND imaa001 = inag001 " +
                "      LEFT JOIN inayl_t ON inaylent = inagent AND inayl001 = inag004 AND inayl002 = 'zh_CN' " +
                "      LEFT JOIN imaal_t ON imaalent = inagent AND imaal001 = inag001 AND imaal002 = 'zh_CN' " +
                "      LEFT JOIN inaml_t ON inamlent = inagent AND inaml001 = inag001  " +
                "            AND inaml002 = inag002 AND inaml003 = 'zh_CN' " +
                "      LEFT JOIN ( SELECT tm.ent ent,tm.site SITE, " +
                "                         tm.ITEM_ID lh,tm.ITEM_FEATURE cptz, " +
                "                         tm.STOCK_FEATURE kctz,tm.LOT_NUMBER ph, " +
                "                         tm.BAR_DATE tmDate,kc.STOREHOUSE kw,kc.STORAGE cw" +
                "                        ,tm.department dp " +
                "                    FROM lrjas.dic_barcode tm " +
                "                   INNER JOIN lrjas.dic_barstock kc ON tm.ent = kc.ent  " +
                "                          AND tm.site = kc.site AND tm.BAR_CODE = kc.Bar_Code " +
                "                          AND kc.qty >0 AND tm.stus = 'Y' ) tmb " +
                "                           ON tmb.ent = inagent AND tmb.site = inagsite  " +
                "                          AND tmb.lh = inag001 AND tmb.cptz = inag002 " +
                "                          AND tmb.kctz = inag003 AND tmb.kw = inag004 " +
                "                          AND tmb.ph = inag006  " +
                "  WHERE inag008 > 0  " +
                "    AND inagent = 1 " +
                "    AND inagsite IN ('SHNAR','NTBN') ";
            string where = QWhere(site,type,dayType);
            _sql += where;

            _sqlExcel = _sql;

            _sql = "SELECT * FROM (" + _sql + " ) " +
                   " WHERE rm between "+(pageIndex*100-99)+" AND "+(pageIndex*100);

            return _sql;
        }

        //excel导出全部数据专用
        public static string SqlExcel(string site, string type, string dayType, int pageIndex)
        {
            MXsql(site, type, dayType, pageIndex);
            return _sqlExcel;
        }

        public static string QWhere(string site, string type, string dayType)
        {
            string where;
            //site 为ALL则说明是Web03.aspx页面右侧的明细
            if(site != "ALL")
            {
                where = " AND inagsite = '"+site+"' ";
            }
            else
            {
                where = " AND 1=1 ";
            }

            //页面类型
            if (type != "ALL")
            {
                switch (type)
                {
                    case "cp": //成品
                        where += " AND ( imaa009 IN ('502','503','506') AND inag004 NOT IN ( '60','53','52') " +
                                 "     OR (imaa009 = '501' AND inag004 NOT IN( '60','53','52') AND SUBSTR(imaa127,1,3) = 'PPF' ) " +
                                 "     OR (inag004 = '52' AND ((substr(inag001,1,1) = '5'  AND imaa009 !='501')  OR (imaa009 = '501' AND imaa127 = 'PPF') ))  " +
                                 "      ) ";
                        break;
                    case "cct"://车贴
                        where += "  AND imaa009 IN ('502','503') " +
                                 "  AND inag004 NOT IN ('52','53','60') ";
                        break;
                    case "cdt"://单透
                        where += "  AND imaa009 = '506' " +
                                 "  AND inag004 NOT IN ('52','53','60') ";
                        break;
                    case "cpp"://PPF
                        where += "  AND imaa009 = '501' " +
                                 "  AND inag004 NOT IN ('52','53','60') ";
                        break;
                    case "cwg"://外购
                        where += "  AND inag004 = '52'  " +
                                 "  AND （ " +
                                 "            (SUBSTR(inag001,1,1) = '5' AND imaa009 != '501') " +
                                 "         OR (imaa009 = '501' AND imaa127 = 'PPF' ) " +
                                 "       ） ";
                        break;

                    case "bcp"://半成品
                        where += " AND ( (substr(inag001,1,1) = '3' AND imaa127 like '%背胶%'AND imaa009 not in('304','321','351','360','322'))  " +
                                 "  OR (substr(inag001,1,1) = '3' AND imaa127 like '%单透%'AND imaa009 not in('304','321','351','360') ) " +
                                 "  OR (imaa009 in ('112','113','114') ) " +
                                 "     ) ";
                        break;
                    case "bbj"://背胶大卷
                        where += " AND substr(inag001,1,1) = '3' AND imaa127 like '%背胶%' AND imaa009 not in('304','321','351','360','322') ";
                        break;
                    case "bdt"://单透大卷
                        where += " AND substr(inag001,1,1) = '3' AND imaa127 like '%单透%' AND imaa009 not in('304','321','351','360') ";
                        break;
                    case "bgz"://硅纸
                        where += " AND imaa009 in ('112','113') ";
                        break;
                    case "bsz"://素纸
                        where += " AND imaa009 = '114' ";
                        break;

                    case "yl"://原料
                        where += "  AND (  " +
                                 "       (imaa009 = '115' OR(SUBSTR(imaa009,1,2) = '14' AND imaa127 NOT LIKE '%胶水%') ) " +
                                 "      OR ( " +
                                 "          (imaa009 IN('101','102') AND ((tmb.dp = '000000' OR tmb.dp IS NULL OR(EXISTS (SELECT 1 FROM ooeg_t WHERE ooegent = 1 AND ooeg001 = tmb.dp)) ))) " +
                                 "          OR ( NOT EXISTS (SELECT 1 FROM ooeg_t WHERE ooegent = 1 AND ooeg001 = tmb.dp)) )  " +
                                 "     ) ";
                        break;
                    case "yyz": //原纸
                        where += " AND imaa009 = '115' ";
                        break;
                    case "yhg": //化工
                        where += " AND SUBSTR(imaa009,1,2) = '14' AND imaa127 NOT LIKE '%胶水%' ";
                        break;
                    case "yjs": //胶水
                        where += " AND SUBSTR(imaa009,1,2) = '14' AND imaa127 LIKE '%胶水%' ";
                        break;
                    case "ymz": //膜(自产)
                        where += " AND imaa009 IN('101','102') AND(tmb.dp = '000000' OR tmb.dp IS NULL OR(EXISTS (SELECT 1 FROM ooeg_t WHERE ooegent = 1 AND ooeg001 = tmb.dp))) ";
                        break;
                    case "ymw": //膜(外购)
                        where += " AND imaa009 IN('101','102') AND NOT EXISTS (SELECT 1 FROM ooeg_t WHERE ooegent = 1 AND ooeg001 = tmb.dp) ";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                where += " AND 1=1 ";
            }

            //dayType为ALL 左侧明细数据,不需要分时间段
            if (dayType != "ALL")
            {
                switch (dayType)
                {
                    case "a":  //1-15天
                        where = where + " AND (TRUNC(SYSDATE) - tmb.tmdate) BETWEEN 1 and 15 ";
                        break;
                    case "b":  //16-30天
                        where = where + " AND (TRUNC(SYSDATE) - tmb.tmdate) BETWEEN 16 and 30 ";
                        break;
                    case "c":  //31-90天
                        where = where + " AND (TRUNC(SYSDATE) - tmb.tmdate) BETWEEN 31 and 90 ";
                        break;
                    case "d":  //91-180
                        where = where + " AND (TRUNC(SYSDATE) - tmb.tmdate) BETWEEN 91 and 180 ";
                        break;
                    case "e": // >180
                        where = where + " AND (TRUNC(SYSDATE) - tmb.tmdate) >= 181 ";
                        break;
                    case "f": // 其他
                        where = where + " AND tmb.tmdate IS NULL ";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                where += " AND 1=1 ";
            }
            return where;
        }
        /// <summary>
        /// Web03页面左侧扇形图数据来源
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string Web03left(string type)
        {
            switch (type)
            {
                case "cp"://成品
                    _sql = "  SELECT SHNAR,NTBN " +
                           "    FROM ( " +
                           "            SELECT inagsite, inag008 " +
                           "              FROM inag_t  " +
                           "                   INNER JOIN imaa_t ON inagent = imaaent  AND inag001 = imaa001 " +
                           "             WHERE inagent = 1 " +
                           "               AND inagsite IN ('SHNAR','NTBN') " +
                           "               AND inag008 >0" +
                           "               AND ( imaa009 IN ('502','503','506') AND inag004 NOT IN ( '60','53','52') " +
                           "                    OR (imaa009 = '501' AND inag004 NOT IN( '60','53','52') AND SUBSTR(imaa127,1,3) = 'PPF' ) " +
                           "                    OR (inag004 = '52' AND ((substr(inag001,1,1) = '5'  AND imaa009 !='501')  OR (imaa009 = '501' AND imaa127 = 'PPF') ))  " +
                           "                    ) " +
                           "        ) " +
                           "   PIVOT ( SUM(inag008) FOR inagsite IN ('SHNAR' AS SHNAR,'NTBN' AS NTBN)) ";
                    break;
                case "bcp"://半成品
                    _sql = "  SELECT SHNAR,NTBN " +
                           "    FROM ( " +
                           "            SELECT inagsite, inag008 " +
                           "              FROM inag_t  " +
                           "                   INNER JOIN imaa_t ON inagent = imaaent  AND inag001 = imaa001 " +
                           "             WHERE inagent = 1 " +
                           "               AND inagsite IN ('SHNAR','NTBN') " +
                           "               AND inag008 >0" +
                           "               AND ( (substr(inag001,1,1) = '3' AND imaa127 like '%背胶%'AND imaa009 not in('304','321','351','360','322'))  " +
                           "                   OR (substr(inag001,1,1) = '3' AND imaa127 like '%单透%'AND imaa009 not in('304','321','351','360') ) " +
                           "                   OR (imaa009 in ('112','113','114') ) " +
                           "                    ) " +
                           "        ) " +
                           "   PIVOT ( SUM(inag008) FOR inagsite IN ('SHNAR' AS SHNAR,'NTBN' AS NTBN)) ";
                    break;
                case "yl"://原料
                    _sql = "  SELECT SHNAR,NTBN " +
                             "    FROM ( " +
                             "            SELECT inagsite, inag008 " +
                             "              FROM inag_t  " +
                             "                   INNER JOIN imaa_t ON inagent = imaaent  AND inag001 = imaa001 " +
                             "             WHERE inagent = 1 " +
                             "               AND inagsite IN ('SHNAR','NTBN') " +
                             "               AND inag008 >0" +
                             "               AND ( imaa009 = '115' OR(SUBSTR(imaa009,1,2) = '14' AND imaa127 NOT LIKE '%胶水%') )" +
                             "         UNION ALL" +
                             "            SELECT kc.site inagsite,kc.qty inag008 " +
                             "              FROM lrjas.dic_barstock kc  " +
                             "                   INNER JOIN lrjas.dic_barcode tm  ON  kc.bar_code = tm.bar_code  AND kc.ent = tm.ent" +
                             "                   INNER JOIN imaa_t ON tm.ent = imaaent  AND tm.item_id = imaa001" +
                             "             WHERE kc.ent = 1 " +
                             "               AND kc.site IN ('SHNAR','NTBN') " +
                             "               AND imaa009 IN('101','102') " +
                             "               AND ((tm.department= '000000' OR tm.department IS NULL" +
                             "                   OR(EXISTS (SELECT 1 FROM ooeg_t WHERE ooegent = 1 AND ooeg001 = tm.department))) " +
                             "                  OR ( NOT EXISTS (SELECT 1 FROM ooeg_t WHERE ooegent = 1 AND ooeg001 = tm.department)) ) " +
                             "        ) " +
                             "   PIVOT ( SUM(inag008) FOR inagsite IN ('SHNAR' AS SHNAR,'NTBN' AS NTBN)) ";
                    break;
            }
           
            return _sql;
        }
        /// <summary>
        /// Web03右侧数据来源
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string Web03Right(string type)
        {
            switch (type)
            {
                #region 成品
                case "cp"://成品
                    _sql = " SELECT NVL(a,0),NVL(b,0),NVL(c,0),NVL(d,0),NVL(e,0),NVL(f,0)  " +
                           "   FROM ( SELECT inag008, " +
                           "                 CASE  WHEN age <= 15 THEN 'a' " +
                           "                       WHEN age > 15 AND age <= 30 THEN 'b'" +
                           "                       WHEN age > 30 AND age <= 90 THEN 'c' " +
                           "                       WHEN age > 90 AND age <= 180 THEN 'd'" +
                           "                       WHEN age > 180 THEN 'e'" +
                           "                       WHEN age IS NULL THEN 'f'" +
                           "                   END dayType " +
                           "            FROM( " +
                           "                  SELECT inag008,(TRUNC(SYSDATE) - tmb.tmdate) age " +
                           "                    FROM inag_t " +
                           "                         INNER JOIN imaa_t ON imaaent = inagent AND imaa001 = inag001 " +
                           "                         LEFT JOIN ( SELECT tm.ent ent,tm.site SITE,tm.ITEM_ID lh,tm.ITEM_FEATURE cptz,tm.STOCK_FEATURE kctz,tm.LOT_NUMBER ph,tm.BAR_DATE tmDate,kc.STOREHOUSE kw,kc.STORAGE cw  " +
                           "                                       FROM lrjas.dic_barcode tm " +
                           "                                       INNER JOIN lrjas.dic_barstock kc ON tm.ent = kc.ent  AND tm.site = kc.site AND tm.BAR_CODE = kc.Bar_Code  AND kc.qty >0 AND tm.stus = 'Y' ) tmb ON tmb.ent = inagent AND tmb.site = inagsite  " +
                           "                                       AND tmb.lh = inag001 AND tmb.cptz = inag002 AND tmb.kctz = inag003 AND tmb.kw = inag004  AND tmb.ph = inag006  " +
                           "                   WHERE inagent = 1 " +
                           "                     AND inagsite IN ('SHNAR','NTBN') " +
                           "                     AND inag008 > 0 " +
                           "                     AND ( imaa009 IN ('502','503','506') AND inag004 NOT IN ( '60','53','52') OR (imaa009 = '501' AND inag004 NOT IN( '60','53','52') AND SUBSTR(imaa127,1,3) = 'PPF' ) OR (inag004 = '52' AND ((substr(inag001,1,1) = '5'  AND imaa009 !='501')  OR (imaa009 = '501' AND imaa127 = 'PPF') ))  " +
                           "                       ) ) ) " +
                           "  PIVOT (SUM(inag008) FOR dayType IN ('a' AS a,'b' AS b,'c' AS c,'d' AS d,'e' AS e, 'f' AS f) ) ";
                    break;
                #endregion
                #region 半成品
                case "bcp"://半成品
                   _sql = " SELECT NVL(a,0),NVL(b,0),NVL(c,0),NVL(d,0),NVL(e,0),NVL(f,0)  " +
                          "   FROM ( SELECT inag008, " +
                          "                 CASE  WHEN age <= 15 THEN 'a' " +
                          "                       WHEN age > 15 AND age <= 30 THEN 'b'" +
                          "                       WHEN age > 30 AND age <= 90 THEN 'c' " +
                          "                       WHEN age > 90 AND age <= 180 THEN 'd'" +
                          "                       WHEN age > 180 THEN 'e'" +
                          "                       WHEN age IS NULL THEN 'f'" +
                          "                   END dayType " +
                          "            FROM( " +
                          "                  SELECT inag008,(TRUNC(SYSDATE) - tmb.tmdate) age " +
                          "                    FROM inag_t " +
                          "                         INNER JOIN imaa_t ON imaaent = inagent AND imaa001 = inag001 " +
                          "                         LEFT JOIN ( SELECT tm.ent ent,tm.site SITE,tm.ITEM_ID lh,tm.ITEM_FEATURE cptz,tm.STOCK_FEATURE kctz,tm.LOT_NUMBER ph,tm.BAR_DATE tmDate,kc.STOREHOUSE kw,kc.STORAGE cw  " +
                          "                                       FROM lrjas.dic_barcode tm " +
                          "                                       INNER JOIN lrjas.dic_barstock kc ON tm.ent = kc.ent  AND tm.site = kc.site AND tm.BAR_CODE = kc.Bar_Code  AND kc.qty >0 AND tm.stus = 'Y' ) tmb ON tmb.ent = inagent AND tmb.site = inagsite  " +
                          "                                       AND tmb.lh = inag001 AND tmb.cptz = inag002 AND tmb.kctz = inag003 AND tmb.kw = inag004  AND tmb.ph = inag006  " +
                          "                   WHERE inagent = 1 " +
                          "                     AND inagsite IN ('SHNAR','NTBN') " +
                          "                     AND inag008 > 0 " +
                          "                     AND ( (substr(inag001,1,1) = '3' AND imaa127 like '%背胶%'AND imaa009 not in('304','321','351','360','322'))  OR (substr(inag001,1,1) = '3' AND imaa127 like '%单透%'AND imaa009 not in('304','321','351','360') ) OR (imaa009 in ('112','113','114') ) " +
                          "                       ) ) ) " +
                          "  PIVOT (SUM(inag008) FOR dayType IN ('a' AS a,'b' AS b,'c' AS c,'d' AS d,'e' AS e, 'f' AS f) ) ";
                    break;
                #endregion
                #region 原料
                case "yl"://原料
                  _sql = " SELECT NVL(a,0),NVL(b,0),NVL(c,0),NVL(d,0),NVL(e,0),NVL(f,0)  " +
                         "   FROM ( SELECT inag008, " +
                         "                 CASE  WHEN age <= 15 THEN 'a' " +
                         "                       WHEN age > 15 AND age <= 30 THEN 'b'" +
                         "                       WHEN age > 30 AND age <= 90 THEN 'c' " +
                         "                       WHEN age > 90 AND age <= 180 THEN 'd'" +
                         "                       WHEN age > 180 THEN 'e'" +
                         "                       WHEN age IS NULL THEN 'f'" +
                         "                   END dayType " +
                         "            FROM( " +
                         "                  SELECT inag008,(TRUNC(SYSDATE) - tmb.tmdate) age " +
                         "                    FROM inag_t " +
                         "                         INNER JOIN imaa_t ON imaaent = inagent AND imaa001 = inag001 " +
                         "                         LEFT JOIN ( SELECT tm.ent ent,tm.site SITE,tm.ITEM_ID lh,tm.ITEM_FEATURE cptz,tm.STOCK_FEATURE kctz,tm.LOT_NUMBER ph,tm.BAR_DATE tmDate,kc.STOREHOUSE kw,kc.STORAGE cw  " +
                         "                                       FROM lrjas.dic_barcode tm " +
                         "                                       INNER JOIN lrjas.dic_barstock kc ON tm.ent = kc.ent  AND tm.site = kc.site AND tm.BAR_CODE = kc.Bar_Code  AND kc.qty >0 AND tm.stus = 'Y' ) tmb ON tmb.ent = inagent AND tmb.site = inagsite  " +
                         "                                       AND tmb.lh = inag001 AND tmb.cptz = inag002 AND tmb.kctz = inag003 AND tmb.kw = inag004  AND tmb.ph = inag006  " +
                         "                   WHERE inagent = 1 " +
                         "                     AND inagsite IN ('SHNAR','NTBN') " +
                         "                     AND inag008 > 0 " +
                         "                     AND ( imaa009 = '115' OR(SUBSTR(imaa009,1,2) = '14' AND imaa127 NOT LIKE '%胶水%')  ) " +
                         "           UNION ALL " +
                         "              SELECT kc.qty inag008,(TRUNC(SYSDATE) - tm.BAR_DATE) age " +
                         "                FROM lrjas.dic_barstock kc " +
                         "                     INNER JOIN lrjas.dic_barcode tm  ON  kc.bar_code = tm.bar_code  AND kc.ent = tm.ent " +
                         "                     INNER JOIN imaa_t ON tm.ent = imaaent  AND tm.item_id = imaa001" +
                         "               WHERE kc.ent = 1 " +
                         "                 AND kc.site IN ('SHNAR','NTBN') " +
                         "                 AND kc.qty > 0 " +
                         "                 AND imaa009 IN('101','102')  " +
                         "                 AND ((tm.department= '000000' OR tm.department IS NULL OR(EXISTS (SELECT 1 FROM ooeg_t WHERE ooegent = 1 AND ooeg001 = tm.department)) ) OR ( NOT EXISTS (SELECT 1 FROM ooeg_t WHERE ooegent = 1 AND ooeg001 = tm.department))    " +
                         "                       ) ) ) " +
                         "  PIVOT (SUM(inag008) FOR dayType IN ('a' AS a,'b' AS b,'c' AS c,'d' AS d,'e' AS e, 'f' AS f) ) ";
                    break;
                    #endregion 
            }

            return _sql;
        }

        /// <summary>
        /// 成品类扇形图左侧数据来源
        /// 1.车贴
        /// 2.单透
        /// 3.PPF
        /// 4.外购
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static string CPLeft(string site)
        {
            _sql = "  SELECT ct,dt,ppf,wg " +
                   "    FROM (  " +
                   "        SELECT inag008,'ct' atype FROM inag_t INNER JOIN imaa_t ON inagent = imaaent AND inag001 = imaa001 WHERE inagent=1 AND imaa009 IN ('502','503')  AND inag004 not in( '60','53','52')  AND inag008 > 0 " +
                   "           AND inagsite = '"+site+"' " +
                   "     UNION ALL " +
                   "        SELECT inag008,'dt' atype FROM inag_t INNER JOIN imaa_t ON inagent = imaaent AND inag001 = imaa001 WHERE inagent=1 AND inag008 > 0 AND imaa009 = '506' AND inag004 not in( '60','53','52') " +
                   "           AND inagsite = '" + site + "' " +
                   "     UNION ALL " +
                   "        SELECT inag008,'ppf' atype FROM inag_t INNER JOIN imaa_t ON inagent = imaaent AND inag001 = imaa001 WHERE inagent=1 AND inag008 > 0  AND imaa009 = '501' AND inag004 not in( '60','53','52') AND SUBSTR(imaa127,1,3) = 'PPF'  " +
                   "           AND inagsite = '" + site + "' " +
                   "     UNION ALL " +
                   "        SELECT inag008,'wg' atype FROM  inag_t INNER JOIN imaa_t ON inagent = imaaent AND inag001 = imaa001 WHERE inagent=1 AND inag008 > 0  AND inag004 = '52' AND ((substr(inag001,1,1) = '5'  AND imaa009 !='501') OR (imaa009 = '501' AND imaa127 = 'PPF')) " +
                   "           AND inagsite = '" + site + "' " +
                   "       ) " +
                   "   PIVOT (SUM(inag008) FOR atype IN ('ct' AS ct,'dt' AS dt,'ppf' AS ppf,'wg' AS wg)) ";
                   
            return _sql;
        }
        /// <summary>
        /// 成品右侧页面数据源
        /// 1-15 
        /// 16-30 
        /// 31-90 
        /// 90-180 
        /// >180
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static string CPRight(string site)
        {
            _sql = " SELECT NVL(a,0),NVL(b,0),NVL(c,0),NVL(d,0),NVL(e,0),NVL(f,0)  " +
                           "   FROM ( SELECT inag008, " +
                           "                 CASE  WHEN age <= 15 THEN 'a' " +
                           "                       WHEN age > 15 AND age <= 30 THEN 'b'" +
                           "                       WHEN age > 30 AND age <= 90 THEN 'c' " +
                           "                       WHEN age > 90 AND age <= 180 THEN 'd'" +
                           "                       WHEN age > 180 THEN 'e'" +
                           "                       WHEN age IS NULL THEN 'f'" +
                           "                   END dayType " +
                           "            FROM( " +
                           "                  SELECT inag008,(TRUNC(SYSDATE) - tmb.tmdate) age " +
                           "                    FROM inag_t " +
                           "                         INNER JOIN imaa_t ON imaaent = inagent AND imaa001 = inag001 " +
                           "                         LEFT JOIN ( SELECT tm.ent ent,tm.site SITE,tm.ITEM_ID lh,tm.ITEM_FEATURE cptz,tm.STOCK_FEATURE kctz,tm.LOT_NUMBER ph,tm.BAR_DATE tmDate,kc.STOREHOUSE kw,kc.STORAGE cw  " +
                           "                                       FROM lrjas.dic_barcode tm " +
                           "                                       INNER JOIN lrjas.dic_barstock kc ON tm.ent = kc.ent  AND tm.site = kc.site AND tm.BAR_CODE = kc.Bar_Code  AND kc.qty >0 AND tm.stus = 'Y' ) tmb ON tmb.ent = inagent AND tmb.site = inagsite  " +
                           "                                       AND tmb.lh = inag001 AND tmb.cptz = inag002 AND tmb.kctz = inag003 AND tmb.kw = inag004  AND tmb.ph = inag006  " +
                           "                   WHERE inagent = 1 " +
                           "                     AND inagsite = '" +site+"' "+
                           "                     AND inag008 > 0 " +
                           "                     AND ( imaa009 IN ('502','503','506') AND inag004 NOT IN ( '60','53','52') OR (imaa009 = '501' AND inag004 NOT IN( '60','53','52') AND SUBSTR(imaa127,1,3) = 'PPF' ) OR (inag004 = '52' AND ((substr(inag001,1,1) = '5'  AND imaa009 !='501')  OR (imaa009 = '501' AND imaa127 = 'PPF') ))  " +
                           "                       ) ) ) " +
                           "  PIVOT (SUM(inag008) FOR dayType IN ('a' AS a,'b' AS b,'c' AS c,'d' AS d,'e' AS e, 'f' AS f) ) ";
            return _sql;
        }

        /// <summary>
        /// 半成品类扇形图左侧数据来源
        /// 1.背胶大卷
        /// 2.单透大卷
        /// 3.硅纸
        /// 4.素纸
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static string BCPLeft(string site)
        {
            _sql = " SELECT bj,dt,gz,sz " +
                "      FROM (  " +
                "           SELECT inag008,'bj' atype FROM inag_t INNER JOIN imaa_t ON inagent = imaaent AND inag001 = imaa001 WHERE inagent = 1 AND inag008 > 0 AND substr(inag001,1,1) = '3' AND imaa127 like '%背胶%' AND imaa009 not in('304','321','351','360','322') " +
                "              AND inagsite = '" + site + "' " +
                "        UNION ALL " +
                "           SELECT inag008,'dt' atype FROM inag_t INNER JOIN imaa_t ON inagent = imaaent AND inag001 = imaa001 WHERE inagent = 1 AND inag008 > 0 AND substr(inag001,1,1) = '3' AND imaa127 like '%单透%'  AND imaa009 not in('304','321','351','360') " +
                "              AND inagsite = '" + site + "' " +
                "        UNION ALL " +
                "           SELECT inag008,'gz' atype FROM inag_t INNER JOIN imaa_t ON inagent = imaaent AND inag001 = imaa001 WHERE inagent = 1 AND inag008 > 0 AND imaa009 in ('112','113')  " +
                "              AND inagsite = '" + site + "' " +
                "        UNION ALL " +
                "           SELECT inag008,'sz' atype FROM inag_t INNER JOIN imaa_t ON inagent = imaaent AND inag001 = imaa001 WHERE inagent = 1 AND inag008 > 0 AND imaa009 = '114'   " +
                "              AND inagsite = '" + site + "' " +
                "           ) " +
                "    PIVOT (SUM(inag008) FOR atype IN ('bj' AS bj,'dt' AS dt,'gz' AS gz,'sz' AS sz)) ";
            return _sql;
        }
        /// <summary>
        /// 半成品右侧页面数据源
        /// 1-15 
        /// 16-30 
        /// 31-90 
        /// 90-180 
        /// >180
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static string BCPRight(string site)
        {
            _sql = " SELECT NVL(a,0),NVL(b,0),NVL(c,0),NVL(d,0),NVL(e,0),NVL(f,0)  " +
                         "   FROM ( SELECT inag008, " +
                         "                 CASE  WHEN age <= 15 THEN 'a' " +
                         "                       WHEN age > 15 AND age <= 30 THEN 'b'" +
                         "                       WHEN age > 30 AND age <= 90 THEN 'c' " +
                         "                       WHEN age > 90 AND age <= 180 THEN 'd'" +
                         "                       WHEN age > 180 THEN 'e'" +
                         "                       WHEN age IS NULL THEN 'f'" +
                         "                   END dayType " +
                         "            FROM( " +
                         "                  SELECT inag008,(TRUNC(SYSDATE) - tmb.tmdate) age " +
                         "                    FROM inag_t " +
                         "                         INNER JOIN imaa_t ON imaaent = inagent AND imaa001 = inag001 " +
                         "                         LEFT JOIN ( SELECT tm.ent ent,tm.site SITE,tm.ITEM_ID lh,tm.ITEM_FEATURE cptz,tm.STOCK_FEATURE kctz,tm.LOT_NUMBER ph,tm.BAR_DATE tmDate,kc.STOREHOUSE kw,kc.STORAGE cw  " +
                         "                                       FROM lrjas.dic_barcode tm " +
                         "                                       INNER JOIN lrjas.dic_barstock kc ON tm.ent = kc.ent  AND tm.site = kc.site AND tm.BAR_CODE = kc.Bar_Code  AND kc.qty >0 AND tm.stus = 'Y' ) tmb ON tmb.ent = inagent AND tmb.site = inagsite  " +
                         "                                       AND tmb.lh = inag001 AND tmb.cptz = inag002 AND tmb.kctz = inag003 AND tmb.kw = inag004  AND tmb.ph = inag006  " +
                         "                   WHERE inagent = 1 " +
                         "                     AND inagsite = '" +site+"' " +
                         "                     AND inag008 > 0 " +
                         "                     AND ( (substr(inag001,1,1) = '3' AND imaa127 like '%背胶%'AND imaa009 not in('304','321','351','360','322'))  OR (substr(inag001,1,1) = '3' AND imaa127 like '%单透%'AND imaa009 not in('304','321','351','360') ) OR (imaa009 in ('112','113','114') ) " +
                         "                       ) ) ) " +
                         "  PIVOT (SUM(inag008) FOR dayType IN ('a' AS a,'b' AS b,'c' AS c,'d' AS d,'e' AS e, 'f' AS f) ) ";
            return _sql;
        }


        /// <summary>
        /// 原料类扇形图左侧数据来源
        /// 1.原纸
        /// 2.化工
        /// 3.胶水
        /// 4.膜(自产)
        /// 5.膜(外购)
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static string YLLeft(string site)
        {
            _sql = " SELECT yz,hg,js,mzc,mwg " +
                   "   FROM (  " +
                   "        SELECT inag008,'yz' atype FROM inag_t INNER JOIN imaa_t ON inagent = imaaent AND inag001 = imaa001 WHERE inagent = 1 AND inag008 > 0 AND imaa009 = '115' " +
                   "           AND inagsite =  '"+site+"'  " +
                   "     UNION ALL " +
                   "        SELECT inag008,'hg' atype FROM inag_t INNER JOIN imaa_t ON inagent = imaaent AND inag001 = imaa001 WHERE inagent = 1 AND inag008 > 0 AND SUBSTR(imaa009,1,2) = '14' AND imaa127 NOT LIKE '%胶水%' " +
                   "           AND inagsite =  '" + site + "'  " +
                   "     UNION ALL " +
                   "        SELECT inag008,'js' atype FROM inag_t INNER JOIN imaa_t ON inagent = imaaent AND inag001 = imaa001 WHERE inagent = 1 AND inag008 > 0 AND SUBSTR(imaa009,1,2) = '14' AND imaa127 LIKE '%胶水%' " +
                   "           AND inagsite =  '" + site + "'  " +
                   "     UNION ALL " +
                   "        SELECT kc.qty inag008,'mzc' atype FROM lrjas.dic_barstock kc INNER JOIN lrjas.dic_barcode tm  ON  kc.bar_code = tm.bar_code  AND kc.ent = tm.ent INNER JOIN imaa_t ON tm.ent = imaaent  AND tm.item_id = imaa001 WHERE kc.qty >0  AND kc.ent = 1 AND imaa009 IN('101','102') AND (tm.department= '000000' OR tm.department IS NULL OR(EXISTS (SELECT 1 FROM ooeg_t WHERE ooegent = 1 AND ooeg001 = tm.department)))  " +
                   "           AND kc.site = '" +site+"' " +
                   "     UNION ALL " +
                   "        SELECT kc.qty inag008,'mwg' atype FROM lrjas.dic_barstock kc INNER JOIN lrjas.dic_barcode tm  ON  kc.bar_code = tm.bar_code  AND kc.ent = tm.ent INNER JOIN imaa_t ON tm.ent = imaaent  AND tm.item_id = imaa001 WHERE kc.qty >0 AND kc.ent = 1 AND imaa009 IN('101','102') AND NOT EXISTS (SELECT 1 FROM ooeg_t WHERE ooegent = 1 AND ooeg001 = tm.department) " +
                   "           AND kc.site = '" + site + "' " +
                   "       ) " +
                   "  PIVOT (SUM(inag008) FOR atype IN ('yz' AS yz,'hg' AS hg,'js' AS js,'mzc' AS mzc,'mwg' AS mwg )) ";

            return _sql;
        }
        /// <summary>
        /// 原料右侧页面数据源
        /// 1-15 
        /// 16-30 
        /// 31-90 
        /// 90-180 
        /// >180
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static string YLRight(string site)
        {
            _sql = " SELECT NVL(a,0),NVL(b,0),NVL(c,0),NVL(d,0),NVL(e,0),NVL(f,0)  " +
                        "   FROM ( SELECT inag008, " +
                        "                 CASE  WHEN age <= 15 THEN 'a' " +
                        "                       WHEN age > 15 AND age <= 30 THEN 'b'" +
                        "                       WHEN age > 30 AND age <= 90 THEN 'c' " +
                        "                       WHEN age > 90 AND age <= 180 THEN 'd'" +
                        "                       WHEN age > 180 THEN 'e'" +
                        "                       WHEN age IS NULL THEN 'f'" +
                        "                   END dayType " +
                        "            FROM( " +
                        "                  SELECT inag008,(TRUNC(SYSDATE) - tmb.tmdate) age " +
                        "                    FROM inag_t " +
                        "                         INNER JOIN imaa_t ON imaaent = inagent AND imaa001 = inag001 " +
                        "                         LEFT JOIN ( SELECT tm.ent ent,tm.site SITE,tm.ITEM_ID lh,tm.ITEM_FEATURE cptz,tm.STOCK_FEATURE kctz,tm.LOT_NUMBER ph,tm.BAR_DATE tmDate,kc.STOREHOUSE kw,kc.STORAGE cw  " +
                        "                                       FROM lrjas.dic_barcode tm " +
                        "                                       INNER JOIN lrjas.dic_barstock kc ON tm.ent = kc.ent  AND tm.site = kc.site AND tm.BAR_CODE = kc.Bar_Code  AND kc.qty >0 AND tm.stus = 'Y' ) tmb ON tmb.ent = inagent AND tmb.site = inagsite  " +
                        "                                       AND tmb.lh = inag001 AND tmb.cptz = inag002 AND tmb.kctz = inag003 AND tmb.kw = inag004  AND tmb.ph = inag006  " +
                        "                   WHERE inagent = 1 " +
                        "                     AND inagsite IN ('SHNAR','NTBN') " +
                        "                     AND inag008 > 0 " +
                        "                     AND ( imaa009 = '115' OR(SUBSTR(imaa009,1,2) = '14' AND imaa127 NOT LIKE '%胶水%')  ) " +
                        "           UNION ALL " +
                        "              SELECT kc.qty inag008,(TRUNC(SYSDATE) - tm.BAR_DATE) age " +
                        "                FROM lrjas.dic_barstock kc " +
                        "                     INNER JOIN lrjas.dic_barcode tm  ON  kc.bar_code = tm.bar_code  AND kc.ent = tm.ent " +
                        "                     INNER JOIN imaa_t ON tm.ent = imaaent  AND tm.item_id = imaa001" +
                        "               WHERE kc.ent = 1 " +
                        "                 AND kc.site = '" +site+"' "+
                        "                 AND kc.qty > 0 " +
                        "                 AND imaa009 IN('101','102')  " +
                        "                 AND ((tm.department= '000000' OR tm.department IS NULL OR(EXISTS (SELECT 1 FROM ooeg_t WHERE ooegent = 1 AND ooeg001 = tm.department)) ) OR ( NOT EXISTS (SELECT 1 FROM ooeg_t WHERE ooegent = 1 AND ooeg001 = tm.department))    " +
                        "                       ) ) ) " +
                        "  PIVOT (SUM(inag008) FOR dayType IN ('a' AS a,'b' AS b,'c' AS c,'d' AS d,'e' AS e, 'f' AS f) ) ";
            return _sql;
        }

        public static string Web03FY(string site, string type, string dayType)
        {
            
            string sql = " SELECT CEIL(count(1)/100) " +
                         "   FROM ( ";


            sql += " SELECT ROWNUM rm,inagsite,inag004,inayl003,inag001,imaal003,imaal004,inag002,inaml004,  " +
                "           inag003,NVL(inag008,0)inag008,inag007,nvl(inag025,0) inag025,inag024,(TRUNC(SYSDATE) - tmb.tmdate) age " +
                "      FROM inag_t " +
                "     INNER JOIN imaa_t ON imaaent = inagent AND imaa001 = inag001 " +
                "      LEFT JOIN inayl_t ON inaylent = inagent AND inayl001 = inag004 AND inayl002 = 'zh_CN' " +
                "      LEFT JOIN imaal_t ON imaalent = inagent AND imaal001 = inag001 AND imaal002 = 'zh_CN' " +
                "      LEFT JOIN inaml_t ON inamlent = inagent AND inaml001 = inag001  " +
                "            AND inaml002 = inag002 AND inaml003 = 'zh_CN' " +
                "      LEFT JOIN ( SELECT tm.ent ent,tm.site SITE, " +
                "                         tm.ITEM_ID lh,tm.ITEM_FEATURE cptz, " +
                "                         tm.STOCK_FEATURE kctz,tm.LOT_NUMBER ph, " +
                "                         tm.BAR_DATE tmDate,kc.STOREHOUSE kw,kc.STORAGE cw" +
                "                        ,tm.department dp " +
                "                    FROM lrjas.dic_barcode tm " +
                "                   INNER JOIN lrjas.dic_barstock kc ON tm.ent = kc.ent  " +
                "                          AND tm.site = kc.site AND tm.BAR_CODE = kc.Bar_Code " +
                "                          AND kc.qty >0 AND tm.stus = 'Y' ) tmb " +
                "                           ON tmb.ent = inagent AND tmb.site = inagsite  " +
                "                          AND tmb.lh = inag001 AND tmb.cptz = inag002 " +
                "                          AND tmb.kctz = inag003 AND tmb.kw = inag004 " +
                "                          AND tmb.ph = inag006  " +
                "  WHERE inag008 > 0  " +
                "    AND inagent = 1 " +
                "    AND inagsite IN ('SHNAR','NTBN') ";
            string where = QWhere(site, type, dayType);
            sql += where;
            sql += " ) ";
            return sql;
        }

    }
}