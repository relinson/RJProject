using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRO_ReceiptsInvMgr.Core.Helper
{ /// <summary>
  /// 新建日期：2017-07-11
  /// 功能描述：获取金税盘的的发票数据
  /// </summary>
    public static class XmlParameters
    {

        /// <summary>
        /// 企业信息及平台信息查询接口
        /// </summary>

        public const string REQUEST_QUERYMAIN = "REQUEST_QUERYMAIN";
        //发票签章 发票开具信息节点
        public const string response_FPKJFPT = "//COMMON_FPKJ_FPT";
        //下次发票查询时间
        public const string RESPONSE_XCFPCXQSSJ = "RESPONSE_XCFPCXQSSJ";

        //主查询返回的发票节点
        public const string RESPONSE_JL = "RESPONSE_JL";

        //OCX组件获取企业信息节点
        public const string response_ENTINFO = "//RESPONSE_ENTINFO";

        //QueryQYInfo查询采集企业信息XML格式模板
        public const string QueryQYXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root>" +
              @"<REQUEST_QUERYMAIN class=""REQUEST_QUERYMAIN""><PROTOCOLINFO class=""PROTOCOLINFO"">" +
              @"<PROTOCOLVER>1.0</PROTOCOLVER>" +//协议版本号，当前为1.0
                 @"</PROTOCOLINFO>" +
              @"<REQUEST_INFO class=""REQUEST_INFO"">" +
              @"<NSRSBH>{0}</NSRSBH>" +//纳税人识别号
              @"<FJH>{1}</FJH>" +//<!--分机号-->
              @"<ZDH>0</ZDH>" +//<!--终端号-->
              @"<MBLJ>{2}</MBLJ>" + //开票软件运行程序所在路径E:\开票软件\150000106211289536.0\Bin-->
              @"<BYZD1></BYZD1>" +
              @"<BYZD2></BYZD2>" +
              @"<BYZD3></BYZD3>" +
          @"</REQUEST_INFO></REQUEST_QUERYMAIN></root>";

        //QueryMainInfo主要发票信息查询XML格式模板
        public const string QueryMainXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root>" +
              @"<REQUEST_QUERYMAIN class=""REQUEST_QUERYMAIN""><PROTOCOLINFO class=""PROTOCOLINFO"">" +
              @"<PROTOCOLVER>1.0</PROTOCOLVER>" +//协议版本号，当前为1.0
                 @"</PROTOCOLINFO>" +
              @"<REQUEST_INFO class=""REQUEST_INFO"">" +
              @"<NSRSBH>{0}</NSRSBH>" +//纳税人识别号
              @"<FJH>{1}</FJH>" +//<!--分机号-->
              @"<ZDH>0</ZDH>" +//<!--终端号-->
              @"<MBLJ>{2}</MBLJ>" + //开票软件运行程序所在路径E:\开票软件\150000106211289536.0\Bin-->
              @"<QSSJ>{3}</QSSJ>" + //起始时间。yyyy-mm-dd hh:mm:ss2011-01-06 00:00:00-->
              @"<ZZSJ>{4}</ZZSJ>" + //终止时间。yyyy-mm-dd hh:mm:ss-->
              @"<PZBZ>51</PZBZ>" + //默认为空，票种标志
              @"<BYZD1></BYZD1>" +
              @"<BYZD2></BYZD2>" +
              @"<BYZD3></BYZD3>" +
          @"</REQUEST_INFO></REQUEST_QUERYMAIN></root>";

        //补充查询模板
        public const string PSQueryXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root>" +
            @"<REQUEST_QUERYMAIN class=""REQUEST_QUERYMAIN""><PROTOCOLINFO class=""PROTOCOLINFO"">" +
            @"<PROTOCOLVER>1.0</PROTOCOLVER>" +//协议版本号，当前为1.0
               @"</PROTOCOLINFO>" +
            @"<REQUEST_INFO class=""REQUEST_INFO"">" +
            @"<NSRSBH>{0}</NSRSBH>" +//纳税人识别号
            @"<FJH>{1}</FJH>" +//<!--分机号-->
            @"<ZDH>0</ZDH>" +//<!--终端号-->
            @"<MBLJ>{2}</MBLJ>" + //开票软件运行程序所在路径E:\开票软件\150000106211289536.0\Bin-->
            @"<PZBZ></PZBZ>" + //默认为空，票种标志
            @"<BYZD1> BYZD1</BYZD1>" +
            @"<BYZD2>BYZD2</BYZD2>" +
            @"<BYZD3>BYZD3</BYZD3>" +
        @"</REQUEST_INFO></REQUEST_QUERYMAIN></root>";

        //明细查询通用报文模板
        public const string DetailQueryFramework = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root>" +
           @"<REQUEST_QUERYDETAIL class=""REQUEST_QUERYDETAIL""><PROTOCOLINFO class=""PROTOCOLINFO"">" +
           @"<PROTOCOLVER>1.0</PROTOCOLVER>" +//协议版本号，当前为1.0
              @"</PROTOCOLINFO>" +
           @"<REQUEST_XXS class=""REQUEST_XX;"" size=""{0}"">" +
           @"{1}" +//纳税人识别号

       @"</REQUEST_XXS></REQUEST_QUERYDETAIL></root>";

        //明细查询内容模板
        public const string DetailQueryXML = "<REQUEST_XX>" +
               @"<NSRSBH>{0}</NSRSBH>" +//<!--纳税人识别号-->
               @"<FJH>{1}</FJH>" +//<!--分机号-->
               @"<ZDH>0</ZDH>" +//<!--终端号-->
               @"<MBLJ>{2}</MBLJ>" +//<!--开票软件运行程序所在路径-->
               @"<FPDM>{3}</FPDM>" +//<!--发票代码-->
               @"<FPHM>{4}</FPHM>" +//<!--发票号码-->
           @"</REQUEST_XX>";


    }
}

