CREATE TABLE IF NOT EXISTS  [T_Config] (
    [ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT, 
	[NSRSBH] nvarchar(50), 
	[NSRMC] nvarchar(254), 
	[DQDM] nvarchar(254), 
	[LoginPwd] nvarchar(254), 
	[CertPwd] nvarchar(254),
	[TaxType] int,
	[IsRemember] bit
);


CREATE TABLE IF NOT EXISTS  [DbVersion] (
[ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT,
[Version] nvarchar(20)
);

INSERT INTO [DbVersion] ([Version]) 
VALUES ('1.0.0');
  
CREATE TABLE IF NOT EXISTS  [SoftwareVersion]  (
ID integer NOT NULL PRIMARY KEY AUTOINCREMENT,
LBBM integer NULL,--软件类别编码
[Version] nvarchar(20) NULL,--软件版本号
Name nvarchar(100) NULL,--软件名称
[DowloadPath] nvarchar(200) NULL,--软件路径
OPERATEDATE date NULL,--软件路径
IsDownComplete bool NULL,
IsForceUpdate bool null
 );
   
CREATE TABLE  IF NOT EXISTS  [T_Fpcy] (
	[ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT, 
	[InvoiceCode] nvarchar(50), 
	[InvoiceNo] nvarchar(50), 
	[InvoiceDate] nvarchar(50), 
	[TotalAmount] double,
	[CheckCode] nvarchar(50), 
	[InvoiceType] nvarchar(50), 
	[InvoiceData] nvarchar(50000),
	[OperateDate] datetime,
	[ResultCode] nvarchar(50)
);
 
CREATE TABLE  IF NOT EXISTS  [T_Dqdm] (
	[ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT,
	[DqCode] nvarchar(50),
	[ProviceName] nvarchar(50),
	[NetLocation] nvarchar(50)
);

INSERT INTO [T_Dqdm] VALUES ('1','1100','北京','https://fpdk.bjsat.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('2','1200','天津','https://fpdk.tjsat.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('3','1300','河北','https://fpdk.he-n-tax.gov.cn:81/');
INSERT INTO [T_Dqdm] VALUES ('4','1400','山西','https://fpdk.sx-n-tax.gov.cn');
INSERT INTO [T_Dqdm] VALUES ('5','1500','内蒙古','https://fpdk.nm-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('6','2100','辽宁','https://fpdk.tax.ln.cn/');
INSERT INTO [T_Dqdm] VALUES ('7','2102','大连','https://fpdk.dlntax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('8','2200','吉林','https://fpdk.jl-n-tax.gov.cn:4431/');
INSERT INTO [T_Dqdm] VALUES ('9','2300','黑龙江','https://fpdk.hl-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('10','3100','上海','https://fpdk.tax.sh.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('11','3200','江苏','https://fpdk.jsgs.gov.cn:81/');
INSERT INTO [T_Dqdm] VALUES ('12','3300','浙江','https://fpdk.zjtax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('13','3302','宁波','https://fpdk.nb-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('14','3400','安徽','https://fpdk.ah-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('15','3500','福建','https://fpdk.fj-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('16','3502','厦门','https://fpdk.xm-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('17','3600','江西','https://fpdk.jxgs.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('18','3700','山东','https://fpdk.sd-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('19','3702','青岛','https://fpdk.qd-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('20','4100','河南','https://fpdk.ha-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('21','4200','湖北','https://fpdk.hb-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('22','4300','湖南','https://fpdk.hntax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('23','4400','广东','https://fpdk.gd-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('24','4403','深圳','https://fpdk.szgs.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('25','4500','广西','https://fpdk.gxgs.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('26','4600','海南','https://fpdk.hitax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('27','5000','重庆','https://fpdk.cqsw.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('28','5100','四川','https://fpdk.sc-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('29','5200','贵州','https://fpdk.gz-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('30','5300','云南','https://fpdk.yngs.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('31','5400','西藏','https://fpdk.xztax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('32','6100','陕西','https://fprzweb.sn-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('33','6200','甘肃','https://fpdk.gs-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('34','6300','青海','http://fpdk.qh-n-tax.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('35','6400','宁夏','https://fpdk.nxgs.gov.cn/');
INSERT INTO [T_Dqdm] VALUES ('36','6500','新疆','https://fpdk.xj-n-tax.gov.cn/');
 