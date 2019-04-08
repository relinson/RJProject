using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_ReceiptsInvMgr.Domain.Enum
{
    /// <summary>
    /// 接口枚举
    /// </summary>
    public enum InterfaceType
    {
        /// <summary>
        /// 注册
        /// </summary>
        [Description("注册")]
        Register = 0,
        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        Login = 1,
        /// <summary>
        /// 软件下载
        /// </summary>
        [Description("软件下载")]
        DownLoad = 2,
        /// <summary>
        /// 软件版本查询
        /// </summary>
        [Description("软件版本查询")]
        CX = 3,

        /// <summary>
        /// 发票查验
        /// </summary>
        [Description("发票查验")]
        FPCY = 4,

        /// <summary>
        /// 注册
        /// </summary>
        [Description("注册")]
        JXRegister = 5,

        /// <summary>
        /// 验证是否注册
        /// </summary>
        JXIsRegister = 6,

        /// <summary>
        /// 勾选认证-查询
        /// </summary>
        [Description("勾选认证-查询")]
        JXGXRZSearch = 7,

        /// <summary>
        /// 获取税款所属期
        /// </summary>
        [Description("获取税款所属期")]
        JXSkssq = 8,

        /// <summary>
        /// 获取当期可选发票开票起止日期
        /// </summary>
        [Description("获取当期可选发票开票起止日期")]
        JXKpStartEndDate = 9,

        /// <summary>
        /// 逾期预警
        /// </summary>
        [Description("逾期预警")]
        JXYQYJ = 10,

        /// <summary>
        /// 认证清单
        /// </summary>
        [Description("认证清单")]
        JXRZQD = 11,

        /// <summary>
        /// 逾期提醒月数
        /// </summary>
        [Description("逾期提醒月数")]
        JXYQWarnMonth = 12,

        /// <summary>
        /// 勾选认证
        /// </summary>
        [Description("勾选认证")]
        JXColCert = 13,

        /// <summary>
        /// 地区代码
        /// </summary>
        [Description("地区代码")]
        AreaNo = 14,

        /// <summary>
        /// 应用是否开放
        /// </summary>
        [Description("应用是否开放")]
        AppOpen = 15,

        /// <summary>
        /// 设置逾期提醒月数
        /// </summary>
        [Description("设置逾期提醒月数")]
        JXYQSetWarnMonth = 16,

        /// <summary>
        /// 公告资讯查询
        /// </summary>
        [Description("公告资讯查询")]
        AdvertiseQuery = 17,

        /// <summary>
        /// 修改密码
        /// </summary>
        [Description("修改密码")]
        ModifyPwd = 18,

        /// <summary>
        /// 修改注册码
        /// </summary>
        [Description("修改注册码")]
        ModifyZcm = 19,

        /// <summary>
        /// 常用应用下载查询
        /// </summary>
        [Description("常用应用下载查询")]
        AppDownload = 20,

        /// <summary>
        /// 留言评论查询
        /// </summary>
        [Description("留言评论查询")]
        Lypl = 21,

        /// <summary>
        /// 用户手册下载查询
        /// </summary>
        [Description("用户手册下载查询")]
        ManualDownload = 22,

        /// <summary>
        /// 用户手册下载查询
        /// </summary>
        [Description("勾选认证发票状态")]
        JXInvoiceStatus = 23,
    }

    /// <summary>
    /// 软件更新接口-类别代码枚举
    /// </summary>
    public enum LbblmType
    {
        /// <summary>
        /// 客户端软件
        /// </summary>
        [Description("软件")]
        Client = 3,
    }
    /// <summary>
    /// 软件更新状态
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        ///升级成功 
        /// </summary>
        [Description("升级成功")]
        Success = 1,

        /// <summary>
        /// 升级失败
        /// </summary>
        [Description("升级失败")]
        Fail = 0,
    }

    /// <summary>
    /// 发票上传状态枚举
    /// </summary>
    public enum InvoiceUpType
    {
        /// <summary>
        /// 发票上传成功
        /// </summary>
        [Description("发票上传成功")]
        Sucess = 1,
        /// <summary>
        /// 发票上传失败
        /// </summary>
        [Description("发票上传失败")]
        Fail = -1,
        /// <summary>
        /// 发票未上传
        /// </summary>
        [Description("发票未上传")]
        NotUp = 0,

    }
    /// <summary>
    /// 信息采集登录、登出
    /// </summary>
    public enum InOut
    {
        /// <summary>
        /// 信息采集登录
        /// </summary>
        [Description("信息采集登录")]
        LogIn = 1,
        /// <summary>
        /// 信息采集登出
        /// </summary>
        [Description("信息采集登出")]
        LogOut = 2,

    }

    /// <summary>
    /// 作废发票种类
    /// </summary>
    public enum ZFInvoiceType
    {
        /// <summary>
        /// 空白发票作废
        /// </summary>
        [Description("空白发票作废")]
        BlankInvoice = 3,
        /// <summary>
        /// 正废发票
        /// </summary>
        [Description("正废发票")]
        PositivInvoicee = 4,
        /// <summary>
        /// 负废发票
        /// </summary>
        [Description("负废发票")]
        NegativeInvoice = 5,

    }

    /// <summary>
    /// 正常发票类型
    /// </summary>
    public enum InvoiceType
    {
        /// <summary>
        /// 蓝票
        /// </summary>
        [Description("蓝票")]
        BlueInvoice = 1,
        /// <summary>
        /// 红票
        /// </summary>
        [Description("红票")]
        RedInvoice = 2,
    }

    /// <summary>
    /// 发票标志
    /// </summary>
    public enum InvoiceBz
    {
        /// <summary>
        /// 作废标志
        /// </summary>
        [Description("作废标志")]
        ZFBZ = 1,
        /// <summary>
        /// 修复标志
        /// </summary>
        [Description("修复标志")]
        XFBZ = 1,
    }

    /// <summary>
    /// 发票种类
    /// 发票种类Z0-增专；Z2-增普；41-卷式；51-电子（OCX组件查出来的是0为增专，2为增普）
    /// </summary>
    public enum InvoiceZl
    {
        /// <summary>
        /// 增专
        /// </summary>
        [Description("增专")]
        Z0 = 0,
        /// <summary>
        /// 增普
        /// </summary>
        [Description("增普")]
        Z2 = 2,
        /// <summary>
        /// 卷式
        /// </summary>
        [Description("卷式")]
        JS = 41,
        /// <summary>
        /// 电子
        /// </summary>
        [Description("电子")]
        DZ = 51,

    }

    /// <summary>
    /// 纸质折扣行为4，被折扣行为3，电子发票1折扣行、2被折扣行
    /// </summary>
    public enum Fphxz
    {
        /// <summary>
        /// 电子发票折扣行
        /// </summary>
        [Description("电子发票折扣行")]
        DZZK = 1,
        /// <summary>
        /// 电子发票被折扣行
        /// </summary>
        [Description("电子发票被折扣行")]
        DZBZK = 2,
        /// <summary>
        /// 纸质被折扣行
        /// </summary>
        [Description("纸质被折扣行")]
        PaperBZK = 3,
        /// <summary>
        /// 纸质折扣行
        /// </summary>Z
        [Description("纸质折扣行")]
        PaperZK = 4,
    }

    /// <summary>
    /// 开票设备
    /// </summary>
    public enum DeviceType
    {
        JSP = 1,
        SKP = 2,
    }

    public enum SkpInvoiceType
    {
        /// <summary>
        /// 增值税专用发票
        /// </summary>
        [Description("增专")]
        Z0 = 4,

        [Description("增普")]
        Z2 = 7,

        [Description("卷式")]
        JS = 25,

        [Description("电子")]
        DZ = 26,
    }

    public enum Fpzt
    {
        /// <summary>
        /// 正常
        /// </summary>
        ZC = 0,

        /// <summary>
        /// 作废
        /// </summary>
        ZF = 3,
    }

    /// <summary>
    /// 应用类型下载
    /// </summary>
    public enum DownType
    {
        /// <summary>
        /// 常用应用
        /// </summary>
        [Description("常用应用")]
        App = 1,

        /// <summary>
        /// 打印机驱动
        /// </summary>
        [Description("打印机驱动")]
        Print = 2,

        /// <summary>
        /// 扫描仪驱动
        /// </summary>
        [Description("扫描仪驱动")]
        Scan = 3,
    }

    /// <summary>
    /// 是否强制升级
    /// </summary>
    public enum ForceUpdate
    {
        /// <summary>
        /// 非强制升级
        /// </summary>
        [Description("非强制升级")]
        NotForce = 0,

        /// <summary>
        /// 强制升级 
        /// </summary>
        [Description("强制升级")]
        Force = 1,
    }


    /// <summary>
    /// 查询类型
    /// </summary>
    public enum AdvertiseCxlx
    {
        /// <summary>
        /// 最新资讯
        /// </summary>
        [Description("最新资讯")]
        ZxInfo = 1,

        /// <summary>
        /// 公告展示 
        /// </summary>
        [Description("公告展示")]
        GgInfo = 2,

        /// <summary>
        /// 全部 
        /// </summary>
        [Description("全部")]
        All = 3,
    }


    /// <summary>
    /// 下载软件类型
    /// </summary>
    public enum DownloadType
    {
        /// <summary>
        /// 常用软件
        /// </summary>
        [Description("常用软件")]
        App = 1,

        /// <summary>
        /// 打印机驱动   
        /// </summary>
        [Description("打印机驱动  ")]
        Printer = 2,

        /// <summary>
        /// 扫描仪驱动 
        /// </summary>
        [Description("扫描仪驱动")]
        Scanner = 3,
    }


    /// <summary>
    /// 升级结果
    /// </summary>
    public enum UpdateResult
    {
        fail = 0,
        success = 1,
    }

    public enum StartType
    {
        /// <summary>
        /// 升级客户端
        /// </summary>
        UpdateClient = 1
    }

    /// <summary>
    /// 作废发票种类
    /// </summary>
    public enum FpcyInvoiceType
    {
        /// <summary>
        /// 增值税专用发票
        /// </summary>
        [Description("增值税专用发票")]
        ZYFP = 01,
        /// <summary>
        /// 增值税普通发票
        /// </summary>
        [Description("增值税普通发票")]
        PTFP = 04,
        /// <summary>
        /// 增值税普通发票（电子）
        /// </summary>
        [Description("增值税电子普通发票")]
        DZFP = 10,

        /// <summary>
        /// 增值税普通发票（卷式）
        /// </summary>
        [Description("增值税普通发票（卷式）")]
        GSFP = 11,

        /// <summary>
        /// 通行费发票
        /// </summary>
        [Description("增值税电子普通发票（通行费）")]
        TXFFP = 14,

        /// <summary>
        /// 通行费发票
        /// </summary>
        [Description("机动车销售统一发票")]
        JDC = 03,

        ///// <summary>
        ///// 货运输业增值税专用发票
        ///// </summary>
        //[Description("货运运输业增值税专用发票")]
        //HYYS = 02,
    }


    /// <summary>
    /// 作废发票种类
    /// </summary>
    public enum FpcyResultCode
    {
        /// <summary>
        /// 查验成功发票一致
        /// </summary>
        [Description("查验成功发票一致")]
        CheckSuccess = 0001,

        /// <summary>
        /// 发票信息不一致
        /// </summary>
        [Description("发票信息不一致")]
        CheckInvoiceNotSame = 0006,

        /// <summary>
        /// 所查发票不存在
        /// </summary>
        [Description("所查发票不存在")]
        CheckInvoiceNotExist = 0009,

    }

    /// <summary>
    /// 查验结果
    /// </summary>
    public enum FpcyResult
    {
        SuccessAndSave = 1,

        FailAndSave = 2,

        FailNotSave = 3,
    }

    /// <summary>
    /// 应用编号
    /// </summary>
    public enum AppCode
    {
        /// <summary>
        /// 进项管理
        /// </summary>
        JXGL = 1001,

        /// <summary>
        /// 发票查验
        /// </summary>
        FPCY = 1002
    }

    /// <summary>
    /// 应用类型
    /// </summary>
    public enum AppType
    {
        /// <summary>
        /// 基础应用
        /// </summary>
        Free = 0,

        /// <summary>
        /// 付费应用
        /// </summary>
        Pay = 1,
    }

    /// <summary>
    /// 应用开放状态
    /// </summary>
    public enum AppSate
    {
        /// <summary>
        /// 未开放
        /// </summary>
        NotOpen = 0,

        /// <summary>
        /// 开放
        /// </summary>
        Open = 1,

        /// <summary>
        /// 过期
        /// </summary>
        OverTime = 2,
    }


    /// <summary>
    /// 发票状态
    /// </summary>
    public enum InvoiceStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        ZC = 0,

        /// <summary>
        /// 失控
        /// </summary>
        [Description("失控")]
        SK = 1,

        /// <summary>
        /// 作废
        /// </summary>
        [Description("作废")]
        ZF = 2,

        /// <summary>
        /// 红冲
        /// </summary>
        [Description("红冲")]
        HC = 3,

        /// <summary>
        /// 异常
        /// </summary>
        [Description("异常")]
        YC = 4,
    }
}