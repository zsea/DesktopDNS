using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace DesktopDNS.ViewModels
{
    internal class I18n:ViewModelBase
    {
        public static new I18n i18n { get; }= new I18n();
        private I18n() {
            this.Change("en-US");
            if (!string.IsNullOrWhiteSpace(Server.configure.Language))
            {
                this.Change(Server.configure.Language);
            }
            else {
                this.Change(CultureInfo.InstalledUICulture.Name);
            }
            
        }

        #region 配置项
        public string Menu_Status { get; private set; }
        public string Menu_Settings { get; private set; }
        public string Menu_Logs { get; private set; }
        public string Menu_About { get; private set; }
        public string Menu_Exit { get; private set; }
        public string Status_Status { get; private set; }
        public string Status_Status_Stopped { get; private set; }
        public string Status_Status_Running { get; private set; }
        public string Status_Listen_Port {  get; private set; }
        public string Status_Default_Server {  get; private set; }
        public string Status_Button_Start {  get; private set; }
        public string Status_Button_Stop { get; private set; }
        public string Status_Run_Time { get; private set; }
        public string Status_Unit_Minute { get; private set; }
        public string Status_Unit_Times { get; private set; }

        public string Status_Requested { get; private set; }
        public string Status_Local_Requested {  get; private set; }
        public string Status_Cached { get; private set; }

        public string Settings_Header_Service { get; private set; }
        public string Settings_Header_Group { get; private set; }
        public string Settings_Header_Remote { get; private set; }
        public string Settings_Header_System { get; private set; }

        public string Settings_Service_Port { get; private set; }
        public string Settings_Service_Default_DNS { get; private set; }
        public string Settings_Service_Log_Level { get; private set; }
        public string Settings_Service_Auto_Start { get; private set; }
        public string Settings_Service_Error_IPv4_Invalid { get; private set; }
        public string Settings_Service_Error_Save_Fail { get; private set; }
        public string Settings_Service_Error_Startup_Fail { get; private set; }
        public string Settings_Service_Save_Success { get; private set; }

        public string Boolean_Status_True {  get; private set; }
        public string Boolean_Status_False { get; private set; }

        public string Settings_Button_Service_Save { get; private set; }
        public string Settings_Button_Service_Add { get; private set; }
        public string Settings_Button_Remote_Add { get; private set; }
        public string Settings_Button_System_Save { get;private    set; }

        public string Settings_Group_Col_Name {  get; private set; }
        public string Settings_Group_Col_DNS { get; private set; }
        public string Settings_Group_Col_Status { get; private set; }

        public string Settings_Group_Tooltip_Edit { get; private set; }
        public string Settings_Group_Tooltip_Delete { get; private set; }
        public string Settings_Group_Confirm_Delete {  get; private set; }
        public string Settings_Group_Tooltip_Manage_Domain { get; private set; }
        public string Settings_Group_Window_Add_Title { get; private set; }
        public string Settings_Group_Window_Edit_Title { get; private set; }
        public string Settings_Group_Window_Form_Name {  get; private set; }
        public string Settings_Group_Window_Form_Server { get; private set; }
        public string Settings_Group_Window_Form_Enable {  get; private set; }
        public string Settings_Group_Window_Form_Save {  get; private set; }
        public string Settings_Group_Window_Form_Cancel {  get; private set; }
        public string Settings_Group_Window_Error_Name_Empty { get; private set; }
        public string Settings_Group_Window_Error_DNS_Invalid { get; private set; }
        public string Settings_Group_Window_Error_Name_Exists{ get; private set; }

        public string Settings_Group_Domain_Window_Title { get; private set; }
        public string Settings_Group_Domain_Window_Group_Name { get; private set; }
        public string Settings_Group_Domain_Window_Button_Add { get; private set; }
        public string Settings_Group_Domain_Window_Col_Type { get; private set; }
        public string Settings_Group_Domain_Window_Col_Domain { get; private set; }
        public string Settings_Group_Domain_Window_Col_Value { get; private set; }
        public string Settings_Group_Domain_Window_Col_Server { get; private set; }
        public string Settings_Group_Domain_Window_Col_Match { get; private set; }
        public string Settings_Group_Domain_Window_Col_Status { get; private set; }
        public string Settings_Group_Domain_Window_Tooltip_Edit { get; private set; }
        public string Settings_Group_Domain_Window_Tooltip_Delete { get; private set; }
        public string Settings_Group_Domain_Window_Confirm_Title { get; private set; }
        public string Settings_Group_Domain_Window_Error_Domain_Exists { get; private set; }

        public string Settings_Domain_Mode_FULL { get; private set; }
        public string Settings_Domain_Mode_REGEX { get; private set; }
        public string Settings_Domain_Mode_WILDCARD { get; private set; }
        public string Settings_Domain_Window_Add_Title {  get; private set; }
        public string Settings_Domain_Window_Edit_Title { get; private set; }

        public string Settings_Domain_Window_Form_Type { get; private set; }
        public string Settings_Domain_Window_Form_Domain { get; private set; }
        public string Settings_Domain_Window_Form_Value { get; private set; }
        public string Settings_Domain_Window_Form_Server { get; private set; }
        public string Settings_Domain_Window_Form_Match { get; private set; }
        public string Settings_Domain_Window_Form_Enable { get; private set; }
        public string Settings_Domain_Window_Button_Save { get; private set; }
        public string Settings_Domain_Window_Button_Cancel { get; private set; }
        public string Settings_Domain_Window_Error_Domain_Empty { get; private set; }
        public string Settings_Domain_Window_Error_IPv4_Invalid { get; private set; }
        public string Settings_Domain_Window_Error_Record_Invalid { get; private set; }
        public string Settings_Domain_Window_Error_Exists { get; private set; }
        public string Settings_Remote_Tooltip_Edit { get; private set; }
        public string Settings_Remote_Tooltip_Delete { get; private set; }
        public string Settings_Remote_Unit_Minute {  get; private set; }
        public string Settings_Remote_Confirm_Delete { get; private set; }

        public string Settings_Remote_Window_Add_Title { get; private set; }
        public string Settings_Remote_Window_Edit_Title { get; private set; }
        public string Settings_Remote_Window_Form_Name { get; private set; }
        public string Settings_Remote_Window_Form_URL { get; private set; }
        public string Settings_Remote_Window_Form_Interval { get; private set; }
        public string Settings_Remote_Window_Form_Enable { get; private set; }
        public string Settings_Remote_Window_Button_Save { get; private set; }
        public string Settings_Remote_Window_Button_Cancel { get; private set; }
        public string Settings_Remote_Window_Error_Name_Empty { get; private set; }
        public string Settings_Remote_Window_Error_Url_Invalid { get; private set; }
        public string Settings_Remote_Window_Error_Interval_Invalid { get; private set; }
        public string Settings_Remote_Window_Error_Name_Exists { get; private set; }
        public string Settings_System_Language {  get; private set; }
        public string Settings_Error_Save_Fail { get; private set; }
        public string Settings_Save_Success { get; private set; }

        public string Confirm_Default_Title {  get; private set; }
        public string Confirm_Button_OK { get; private set; }
        public string Confirm_Button_Cancel {  get; private set; }
        public string Confirm_Exit_Message { get; private set; }
        public string Confirm_Stop_Message { get; private set; }
        public string Confirm_Info_Title { get; private set; }
        public string Confirm_Error_Title { get; private set; }
        public string About_Homepage { get; private set; }
        public string About_Version { get; private set; }
        public string About_Thank { get; private set; }

        public string Language_Name { get; private set; }
        #endregion


        public Dictionary<string, string> zhCN_Dict = new Dictionary<string, string>
        {
            // 主菜单
            [nameof(Menu_Status)] = "状态",
            [nameof(Menu_Settings)] = "设置",
            [nameof(Menu_Logs)] = "日志",
            [nameof(Menu_About)] = "关于",
            [nameof(Menu_Exit)]="退出",
            // 状态信息
            [nameof(Status_Status)] = "当前状态",
            [nameof(Status_Status_Stopped)] = "已停止",
            [nameof(Status_Status_Running)] = "运行中",
            [nameof(Status_Listen_Port)] = "监听端口",
            [nameof(Status_Default_Server)] = "默认服务器",
            [nameof(Status_Button_Start)] = "启动",
            [nameof(Status_Button_Stop)] = "停止",
            [nameof(Status_Run_Time)] = "运行时长",
            [nameof(Status_Unit_Minute)] = "分钟",
            [nameof(Status_Unit_Times)] = "次",
            [nameof(Status_Requested)] = "处理请求",
            [nameof(Status_Local_Requested)] = "本地解析",
            [nameof(Status_Cached)] = "缓存数量",

            // 服务设置
            [nameof(Settings_Header_Service)] = "服务设置",
            [nameof(Settings_Header_Group)] = "分组管理",
            [nameof(Settings_Header_Remote)] = "远程规则",
            [nameof(Settings_Header_System)] = "系统设置",
            [nameof(Settings_Service_Port)] = "监听端口",
            [nameof(Settings_Service_Default_DNS)] = "默认DNS服务器",
            [nameof(Settings_Service_Log_Level)] = "日志级别",
            [nameof(Settings_Service_Auto_Start)] = "开机自动启动",
            [nameof(Settings_Service_Error_IPv4_Invalid)] = "默认服务器不是有效的IP地址。",
            [nameof(Settings_Service_Error_Save_Fail)] = "保存成功，配置内容将在下次启动服务时生效。",
            [nameof(Settings_Service_Error_Startup_Fail)] = "服务启动失败。",
            [nameof(Settings_Service_Save_Success)] = "保存成功，配置内容将在下次启动服务时生效。",

            // 布尔状态
            [nameof(Boolean_Status_True)] = "启用",
            [nameof(Boolean_Status_False)] = "禁用",

            // 分组管理 (继续补充剩余项)
            [nameof(Settings_Group_Col_Name)] = "名称",
            [nameof(Settings_Group_Col_DNS)] = "默认服务器",
            [nameof(Settings_Group_Col_Status)] = "状态",
            [nameof(Settings_Group_Tooltip_Edit)] = "编辑",
            [nameof(Settings_Group_Tooltip_Delete)] = "删除",
            [nameof(Settings_Group_Confirm_Delete)] = "你确定要删除分组 [{0}] 吗？",
            [nameof(Settings_Group_Tooltip_Manage_Domain)] = "管理域名",

            // 域名管理
            [nameof(Settings_Group_Domain_Window_Title)] = "域名管理",
            [nameof(Settings_Group_Domain_Window_Group_Name)] = "分组名称",
            [nameof(Settings_Group_Domain_Window_Button_Add)] = "添加",
            [nameof(Settings_Group_Domain_Window_Col_Type)] = "类型",
            [nameof(Settings_Group_Domain_Window_Col_Domain)] = "域名",
            [nameof(Settings_Group_Domain_Window_Col_Value)] = "记录值",
            [nameof(Settings_Group_Domain_Window_Col_Server)] = "解析服务器",
            [nameof(Settings_Group_Domain_Window_Col_Match)] = "匹配",
            [nameof(Settings_Group_Domain_Window_Col_Status)] = "状态",
            [nameof(Settings_Group_Domain_Window_Tooltip_Edit)] = "编辑",
            [nameof(Settings_Group_Domain_Window_Tooltip_Delete)] = "删除",
            [nameof(Settings_Group_Domain_Window_Confirm_Title)] = "你确定要删除域名 [{0}] 的解析吗？",
            [nameof(Settings_Group_Domain_Window_Error_Domain_Exists)] = "不能存在相同的域名规则。",

            // 域名模式
            [nameof(Settings_Domain_Mode_FULL)] = "全等",
            [nameof(Settings_Domain_Mode_REGEX)] = "正则",
            [nameof(Settings_Domain_Mode_WILDCARD)] = "模式匹配",

            // 解析窗口
            [nameof(Settings_Domain_Window_Add_Title)] = "添加解析",
            [nameof(Settings_Domain_Window_Edit_Title)] = "编辑解析",
            [nameof(Settings_Domain_Window_Form_Type)] = "类型",
            [nameof(Settings_Domain_Window_Form_Domain)] = "域名",
            [nameof(Settings_Domain_Window_Form_Value)] = "记录值",
            [nameof(Settings_Domain_Window_Form_Server)] = "解析服务器",
            [nameof(Settings_Domain_Window_Form_Match)] = "匹配模式",
            [nameof(Settings_Domain_Window_Form_Enable)] = "启用",
            [nameof(Settings_Domain_Window_Button_Save)] = "保存",
            [nameof(Settings_Domain_Window_Button_Cancel)] = "取消",
            [nameof(Settings_Domain_Window_Error_Domain_Empty)] = "域名不能为空。",
            [nameof(Settings_Domain_Window_Error_IPv4_Invalid)] = "解析服务器不是一个有效的IPv4地址。",
            [nameof(Settings_Domain_Window_Error_Record_Invalid)] = "记录值不是一个有效的IPv4地址。",
            [nameof(Settings_Domain_Window_Error_Exists)] = "不能存在相同的域名解析记录。",

            // 远程规则 (剩余项继续补充)
            [nameof(Settings_Remote_Tooltip_Edit)] = "编辑",
            [nameof(Settings_Remote_Tooltip_Delete)] = "删除",
            [nameof(Settings_Remote_Unit_Minute)] = "分钟",
            [nameof(Settings_Remote_Confirm_Delete)] = "你确定要删除远程规则 [{0}] 吗？",
            [nameof(Settings_Remote_Window_Add_Title)] = "添加远程规则",
            [nameof(Settings_Remote_Window_Edit_Title)] = "编辑远程规则",
            [nameof(Settings_Remote_Window_Form_Name)] = "名称",
            [nameof(Settings_Remote_Window_Form_URL)] = "URL",
            [nameof(Settings_Remote_Window_Form_Interval)] = "更新间隔(分钟)",
            [nameof(Settings_Remote_Window_Form_Enable)] = "启用",
            [nameof(Settings_Remote_Window_Button_Save)] = "保存",
            [nameof(Settings_Remote_Window_Button_Cancel)] = "取消",
            [nameof(Settings_Remote_Window_Error_Name_Empty)] = "名称不能为空。",
            [nameof(Settings_Remote_Window_Error_Url_Invalid)] = "URL地址无效。",
            [nameof(Settings_Remote_Window_Error_Interval_Invalid)] = "更新间隔时间无效。",
            [nameof(Settings_Remote_Window_Error_Name_Exists)] = "不能存在相同的规则名称。",

            // 系统设置
            [nameof(Settings_System_Language)] = "语言",
            [nameof(Settings_Button_Service_Save)] = "保存",
            [nameof(Settings_Button_Service_Add)] = "添加",
            [nameof(Settings_Button_Remote_Add)] = "添加",
            [nameof(Settings_Button_System_Save)] = "保存",
            [nameof(Settings_Error_Save_Fail)]="保存失败。",
            [nameof(Settings_Save_Success)] = "保存成功。",

            // 确认对话框
            [nameof(Confirm_Default_Title)] = "请确认",
            [nameof(Confirm_Button_OK)] = "确定",
            [nameof(Confirm_Button_Cancel)] = "取消",
            [nameof(Confirm_Exit_Message)] = "DNS服务正在运行，请确认是否退出程序？",
            [nameof(Confirm_Stop_Message)] = "你确定要停止DNS服务吗？",
            [nameof(Confirm_Info_Title)] = "提示",
            [nameof(Confirm_Error_Title)] = "错误",

            // 关于页面
            [nameof(About_Homepage)] = "官网网站：",
            [nameof(About_Version)] = "当前版本：",
            [nameof(About_Thank)] = "感谢以下项目：",

            // 语言标识
            [nameof(Language_Name)] = "简体中文"
        };
        public Dictionary<string, string> enUS_Dict = new Dictionary<string, string>
        {
            // Main Menu
            [nameof(Menu_Status)] = "Status",
            [nameof(Menu_Settings)] = "Settings",
            [nameof(Menu_Logs)] = "Logs",
            [nameof(Menu_About)] = "About",
            [nameof(Menu_Exit)] = "Exit",
            // Status Display
            [nameof(Status_Status)] = "Current Status",
            [nameof(Status_Status_Stopped)] = "Stopped",
            [nameof(Status_Status_Running)] = "Running",
            [nameof(Status_Listen_Port)] = "Listening Port",
            [nameof(Status_Default_Server)] = "Default Server",
            [nameof(Status_Button_Start)] = "Start",
            [nameof(Status_Button_Stop)] = "Stop",
            [nameof(Status_Run_Time)] = "Uptime",
            [nameof(Status_Unit_Minute)] = "minute(s)",
            [nameof(Status_Unit_Times)] = "time(s)",
            [nameof(Status_Requested)] = "Requests Processed",
            [nameof(Status_Local_Requested)] = "Local Resolutions",
            [nameof(Status_Cached)] = "Cached Items",

            // Service Settings
            [nameof(Settings_Header_Service)] = "Service Settings",
            [nameof(Settings_Header_Group)] = "Group Management",
            [nameof(Settings_Header_Remote)] = "Remote Rules",
            [nameof(Settings_Header_System)] = "System Settings",
            [nameof(Settings_Service_Port)] = "Listening Port",
            [nameof(Settings_Service_Default_DNS)] = "Default DNS Server",
            [nameof(Settings_Service_Log_Level)] = "Log Level",
            [nameof(Settings_Service_Auto_Start)] = "Auto-Start on Boot",
            [nameof(Settings_Service_Error_IPv4_Invalid)] = "Default server is not a valid IP address.",
            [nameof(Settings_Service_Error_Save_Fail)] = "Save failed. Configuration will take effect after next service startup.",
            [nameof(Settings_Service_Error_Startup_Fail)] = "Service startup failed.",
            [nameof(Settings_Service_Save_Success)] = "Save successful. Configuration will take effect after next service startup.",

            // Boolean States
            [nameof(Boolean_Status_True)] = "Enabled",
            [nameof(Boolean_Status_False)] = "Disabled",

            // Group Management
            [nameof(Settings_Group_Col_Name)] = "Name",
            [nameof(Settings_Group_Col_DNS)] = "Default Server",
            [nameof(Settings_Group_Col_Status)] = "Status",
            [nameof(Settings_Group_Tooltip_Edit)] = "Edit",
            [nameof(Settings_Group_Tooltip_Delete)] = "Delete",
            [nameof(Settings_Group_Confirm_Delete)] = "Are you sure you want to delete group [{0}]?",
            [nameof(Settings_Group_Tooltip_Manage_Domain)] = "Manage Domains",

            // Group Windows
            [nameof(Settings_Group_Window_Add_Title)] = "Add Group",
            [nameof(Settings_Group_Window_Edit_Title)] = "Edit Group",
            [nameof(Settings_Group_Window_Form_Name)] = "Name",
            [nameof(Settings_Group_Window_Form_Server)] = "Default DNS Server",
            [nameof(Settings_Group_Window_Form_Enable)] = "Enable",
            [nameof(Settings_Group_Window_Form_Save)] = "Save",
            [nameof(Settings_Group_Window_Form_Cancel)] = "Cancel",
            [nameof(Settings_Group_Window_Error_Name_Empty)] = "Name cannot be empty.",
            [nameof(Settings_Group_Window_Error_DNS_Invalid)] = "Invalid default DNS server. Must be a valid IPv4 address.",
            [nameof(Settings_Group_Window_Error_Name_Exists)] = "Group name already exists.",

            // Domain Management
            [nameof(Settings_Group_Domain_Window_Title)] = "Domain Management",
            [nameof(Settings_Group_Domain_Window_Group_Name)] = "Group Name",
            [nameof(Settings_Group_Domain_Window_Button_Add)] = "Add",
            [nameof(Settings_Group_Domain_Window_Col_Type)] = "Type",
            [nameof(Settings_Group_Domain_Window_Col_Domain)] = "Domain",
            [nameof(Settings_Group_Domain_Window_Col_Value)] = "Record Value",
            [nameof(Settings_Group_Domain_Window_Col_Server)] = "Resolver Server",
            [nameof(Settings_Group_Domain_Window_Col_Match)] = "Match Mode",
            [nameof(Settings_Group_Domain_Window_Col_Status)] = "Status",
            [nameof(Settings_Group_Domain_Window_Tooltip_Edit)] = "Edit",
            [nameof(Settings_Group_Domain_Window_Tooltip_Delete)] = "Delete",
            [nameof(Settings_Group_Domain_Window_Confirm_Title)] = "Are you sure you want to delete resolution for domain [{0}]?",
            [nameof(Settings_Group_Domain_Window_Error_Domain_Exists)] = "Domain rule already exists.",

            // Domain Modes
            [nameof(Settings_Domain_Mode_FULL)] = "Exact Match",
            [nameof(Settings_Domain_Mode_REGEX)] = "Regex",
            [nameof(Settings_Domain_Mode_WILDCARD)] = "Wildcard",

            // Resolution Windows
            [nameof(Settings_Domain_Window_Add_Title)] = "Add Resolution",
            [nameof(Settings_Domain_Window_Edit_Title)] = "Edit Resolution",
            [nameof(Settings_Domain_Window_Form_Type)] = "Type",
            [nameof(Settings_Domain_Window_Form_Domain)] = "Domain",
            [nameof(Settings_Domain_Window_Form_Value)] = "Record Value",
            [nameof(Settings_Domain_Window_Form_Server)] = "Resolver Server",
            [nameof(Settings_Domain_Window_Form_Match)] = "Match Mode",
            [nameof(Settings_Domain_Window_Form_Enable)] = "Enable",
            [nameof(Settings_Domain_Window_Button_Save)] = "Save",
            [nameof(Settings_Domain_Window_Button_Cancel)] = "Cancel",
            [nameof(Settings_Domain_Window_Error_Domain_Empty)] = "Domain cannot be empty.",
            [nameof(Settings_Domain_Window_Error_IPv4_Invalid)] = "Resolver server is not a valid IPv4 address.",
            [nameof(Settings_Domain_Window_Error_Record_Invalid)] = "Record value is not a valid IPv4 address.",
            [nameof(Settings_Domain_Window_Error_Exists)] = "Duplicate domain resolution record exists.",

            // Remote Rules
            [nameof(Settings_Remote_Tooltip_Edit)] = "Edit",
            [nameof(Settings_Remote_Tooltip_Delete)] = "Delete",
            [nameof(Settings_Remote_Unit_Minute)] = "minute(s)",
            [nameof(Settings_Remote_Confirm_Delete)] = "Are you sure you want to delete remote rule [{0}]?",
            [nameof(Settings_Remote_Window_Add_Title)] = "Add Remote Rule",
            [nameof(Settings_Remote_Window_Edit_Title)] = "Edit Remote Rule",
            [nameof(Settings_Remote_Window_Form_Name)] = "Name",
            [nameof(Settings_Remote_Window_Form_URL)] = "URL",
            [nameof(Settings_Remote_Window_Form_Interval)] = "Update Interval (minutes)",
            [nameof(Settings_Remote_Window_Form_Enable)] = "Enable",
            [nameof(Settings_Remote_Window_Button_Save)] = "Save",
            [nameof(Settings_Remote_Window_Button_Cancel)] = "Cancel",
            [nameof(Settings_Remote_Window_Error_Name_Empty)] = "Name cannot be empty.",
            [nameof(Settings_Remote_Window_Error_Url_Invalid)] = "Invalid URL address.",
            [nameof(Settings_Remote_Window_Error_Interval_Invalid)] = "Invalid update interval.",
            [nameof(Settings_Remote_Window_Error_Name_Exists)] = "Rule name already exists.",

            // System & Confirmations
            [nameof(Settings_System_Language)] = "Language",
            [nameof(Settings_Button_Service_Save)] = "Save",
            [nameof(Settings_Button_Service_Add)] = "Add",
            [nameof(Settings_Button_Remote_Add)] = "Add",
            [nameof(Settings_Button_System_Save)] = "Save",
            [nameof(Confirm_Default_Title)] = "Confirmation",
            [nameof(Confirm_Button_OK)] = "OK",
            [nameof(Confirm_Button_Cancel)] = "Cancel",
            [nameof(Confirm_Exit_Message)] = "DNS service is running. Are you sure you want to exit?",
            [nameof(Confirm_Stop_Message)] = "Are you sure you want to stop DNS service?",
            [nameof(Confirm_Info_Title)] = "Information",
            [nameof(Confirm_Error_Title)] = "Error",
            [nameof(Settings_Error_Save_Fail)] = "Save failed.",
            [nameof(Settings_Save_Success)] = "Save successful.",
            // About Section
            [nameof(About_Homepage)] = "Website:",
            [nameof(About_Version)] = "Version:",
            [nameof(About_Thank)] = "Special Thanks to:",

            // Language Identifier
            [nameof(Language_Name)] = "English"
        };

        private string GetDictValue(System.Collections.Generic.Dictionary<string, string> dict,string key,string defaultValue)
        {
            if(dict!=null&&dict.ContainsKey(key)) { return dict[key]; }
            return defaultValue;
        }
        private void useDict(System.Collections.Generic.Dictionary<string, string> dict)
        {
            
            // 主選單
            Menu_Status = GetDictValue(dict, nameof(Menu_Status), Menu_Status);
            Menu_Settings = GetDictValue(dict, nameof(Menu_Settings), Menu_Settings);
            Menu_Logs = GetDictValue(dict, nameof(Menu_Logs), Menu_Logs);
            Menu_About = GetDictValue(dict, nameof(Menu_About), Menu_About);
            Menu_Exit = GetDictValue(dict, nameof(Menu_Exit), Menu_Exit);

            // 狀態顯示
            Status_Status = GetDictValue(dict, nameof(Status_Status), Status_Status);
            Status_Status_Stopped = GetDictValue(dict, nameof(Status_Status_Stopped), Status_Status_Stopped);
            Status_Status_Running = GetDictValue(dict, nameof(Status_Status_Running), Status_Status_Running);
            Status_Listen_Port = GetDictValue(dict, nameof(Status_Listen_Port), Status_Listen_Port);
            Status_Default_Server = GetDictValue(dict, nameof(Status_Default_Server), Status_Default_Server);
            Status_Button_Start = GetDictValue(dict, nameof(Status_Button_Start), Status_Button_Start);
            Status_Button_Stop = GetDictValue(dict, nameof(Status_Button_Stop), Status_Button_Stop);
            Status_Run_Time = GetDictValue(dict, nameof(Status_Run_Time), Status_Run_Time);
            Status_Unit_Minute = GetDictValue(dict, nameof(Status_Unit_Minute), Status_Unit_Minute);
            Status_Unit_Times = GetDictValue(dict, nameof(Status_Unit_Times), Status_Unit_Times);
            Status_Requested = GetDictValue(dict, nameof(Status_Requested), Status_Requested);
            Status_Local_Requested = GetDictValue(dict, nameof(Status_Local_Requested), Status_Local_Requested);
            Status_Cached = GetDictValue(dict, nameof(Status_Cached), Status_Cached);

            // 服務設定
            Settings_Header_Service = GetDictValue(dict, nameof(Settings_Header_Service), Settings_Header_Service);
            Settings_Header_Group = GetDictValue(dict, nameof(Settings_Header_Group), Settings_Header_Group);
            Settings_Header_Remote = GetDictValue(dict, nameof(Settings_Header_Remote), Settings_Header_Remote);
            Settings_Header_System = GetDictValue(dict, nameof(Settings_Header_System), Settings_Header_System);
            Settings_Service_Port = GetDictValue(dict, nameof(Settings_Service_Port), Settings_Service_Port);
            Settings_Service_Default_DNS = GetDictValue(dict, nameof(Settings_Service_Default_DNS), Settings_Service_Default_DNS);
            Settings_Service_Log_Level = GetDictValue(dict, nameof(Settings_Service_Log_Level), Settings_Service_Log_Level);
            Settings_Service_Auto_Start = GetDictValue(dict, nameof(Settings_Service_Auto_Start), Settings_Service_Auto_Start);
            Settings_Service_Error_IPv4_Invalid = GetDictValue(dict, nameof(Settings_Service_Error_IPv4_Invalid), Settings_Service_Error_IPv4_Invalid);
            Settings_Service_Error_Save_Fail = GetDictValue(dict, nameof(Settings_Service_Error_Save_Fail), Settings_Service_Error_Save_Fail);
            Settings_Service_Error_Startup_Fail = GetDictValue(dict, nameof(Settings_Service_Error_Startup_Fail), Settings_Service_Error_Startup_Fail);
            Settings_Service_Save_Success = GetDictValue(dict, nameof(Settings_Service_Save_Success), Settings_Service_Save_Success);

            // 布林狀態
            Boolean_Status_True = GetDictValue(dict, nameof(Boolean_Status_True), Boolean_Status_True);
            Boolean_Status_False = GetDictValue(dict, nameof(Boolean_Status_False), Boolean_Status_False);

            // 群組管理
            Settings_Group_Col_Name = GetDictValue(dict, nameof(Settings_Group_Col_Name), Settings_Group_Col_Name);
            Settings_Group_Col_DNS = GetDictValue(dict, nameof(Settings_Group_Col_DNS), Settings_Group_Col_DNS);
            Settings_Group_Col_Status = GetDictValue(dict, nameof(Settings_Group_Col_Status), Settings_Group_Col_Status);
            Settings_Group_Tooltip_Edit = GetDictValue(dict, nameof(Settings_Group_Tooltip_Edit), Settings_Group_Tooltip_Edit);
            Settings_Group_Tooltip_Delete = GetDictValue(dict, nameof(Settings_Group_Tooltip_Delete), Settings_Group_Tooltip_Delete);
            Settings_Group_Confirm_Delete = GetDictValue(dict, nameof(Settings_Group_Confirm_Delete), Settings_Group_Confirm_Delete);
            Settings_Group_Tooltip_Manage_Domain = GetDictValue(dict, nameof(Settings_Group_Tooltip_Manage_Domain), Settings_Group_Tooltip_Manage_Domain);

            // 群組視窗
            Settings_Group_Window_Add_Title = GetDictValue(dict, nameof(Settings_Group_Window_Add_Title), Settings_Group_Window_Add_Title);
            Settings_Group_Window_Edit_Title = GetDictValue(dict, nameof(Settings_Group_Window_Edit_Title), Settings_Group_Window_Edit_Title);
            Settings_Group_Window_Form_Name = GetDictValue(dict, nameof(Settings_Group_Window_Form_Name), Settings_Group_Window_Form_Name);
            Settings_Group_Window_Form_Server = GetDictValue(dict, nameof(Settings_Group_Window_Form_Server), Settings_Group_Window_Form_Server);
            Settings_Group_Window_Form_Enable = GetDictValue(dict, nameof(Settings_Group_Window_Form_Enable), Settings_Group_Window_Form_Enable);
            Settings_Group_Window_Form_Save = GetDictValue(dict, nameof(Settings_Group_Window_Form_Save), Settings_Group_Window_Form_Save);
            Settings_Group_Window_Form_Cancel = GetDictValue(dict, nameof(Settings_Group_Window_Form_Cancel), Settings_Group_Window_Form_Cancel);
            Settings_Group_Window_Error_Name_Empty = GetDictValue(dict, nameof(Settings_Group_Window_Error_Name_Empty), Settings_Group_Window_Error_Name_Empty);
            Settings_Group_Window_Error_DNS_Invalid = GetDictValue(dict, nameof(Settings_Group_Window_Error_DNS_Invalid), Settings_Group_Window_Error_DNS_Invalid);
            Settings_Group_Window_Error_Name_Exists = GetDictValue(dict, nameof(Settings_Group_Window_Error_Name_Exists), Settings_Group_Window_Error_Name_Exists);

            // 網域管理 (以下继续补充剩余项)
            Settings_Group_Domain_Window_Title = GetDictValue(dict, nameof(Settings_Group_Domain_Window_Title), Settings_Group_Domain_Window_Title);
            Settings_Group_Domain_Window_Group_Name = GetDictValue(dict, nameof(Settings_Group_Domain_Window_Group_Name), Settings_Group_Domain_Window_Group_Name);
            Settings_Group_Domain_Window_Button_Add = GetDictValue(dict, nameof(Settings_Group_Domain_Window_Button_Add), Settings_Group_Domain_Window_Button_Add);
            Settings_Group_Domain_Window_Col_Type = GetDictValue(dict, nameof(Settings_Group_Domain_Window_Col_Type), Settings_Group_Domain_Window_Col_Type);
            Settings_Group_Domain_Window_Col_Domain = GetDictValue(dict, nameof(Settings_Group_Domain_Window_Col_Domain), Settings_Group_Domain_Window_Col_Domain);
            Settings_Group_Domain_Window_Col_Value = GetDictValue(dict, nameof(Settings_Group_Domain_Window_Col_Value), Settings_Group_Domain_Window_Col_Value);
            Settings_Group_Domain_Window_Col_Server = GetDictValue(dict, nameof(Settings_Group_Domain_Window_Col_Server), Settings_Group_Domain_Window_Col_Server);
            Settings_Group_Domain_Window_Col_Match = GetDictValue(dict, nameof(Settings_Group_Domain_Window_Col_Match), Settings_Group_Domain_Window_Col_Match);
            Settings_Group_Domain_Window_Col_Status = GetDictValue(dict, nameof(Settings_Group_Domain_Window_Col_Status), Settings_Group_Domain_Window_Col_Status);
            Settings_Group_Domain_Window_Tooltip_Edit = GetDictValue(dict, nameof(Settings_Group_Domain_Window_Tooltip_Edit), Settings_Group_Domain_Window_Tooltip_Edit);
            Settings_Group_Domain_Window_Tooltip_Delete = GetDictValue(dict, nameof(Settings_Group_Domain_Window_Tooltip_Delete), Settings_Group_Domain_Window_Tooltip_Delete);
            Settings_Group_Domain_Window_Confirm_Title = GetDictValue(dict, nameof(Settings_Group_Domain_Window_Confirm_Title), Settings_Group_Domain_Window_Confirm_Title);
            Settings_Group_Domain_Window_Error_Domain_Exists = GetDictValue(dict, nameof(Settings_Group_Domain_Window_Error_Domain_Exists), Settings_Group_Domain_Window_Error_Domain_Exists);

            // 網域模式
            Settings_Domain_Mode_FULL = GetDictValue(dict, nameof(Settings_Domain_Mode_FULL), Settings_Domain_Mode_FULL);
            Settings_Domain_Mode_REGEX = GetDictValue(dict, nameof(Settings_Domain_Mode_REGEX), Settings_Domain_Mode_REGEX);
            Settings_Domain_Mode_WILDCARD = GetDictValue(dict, nameof(Settings_Domain_Mode_WILDCARD), Settings_Domain_Mode_WILDCARD);

            // 解析視窗
            Settings_Domain_Window_Add_Title = GetDictValue(dict, nameof(Settings_Domain_Window_Add_Title), Settings_Domain_Window_Add_Title);
            Settings_Domain_Window_Edit_Title = GetDictValue(dict, nameof(Settings_Domain_Window_Edit_Title), Settings_Domain_Window_Edit_Title);
            Settings_Domain_Window_Form_Type = GetDictValue(dict, nameof(Settings_Domain_Window_Form_Type), Settings_Domain_Window_Form_Type);
            Settings_Domain_Window_Form_Domain = GetDictValue(dict, nameof(Settings_Domain_Window_Form_Domain), Settings_Domain_Window_Form_Domain);
            Settings_Domain_Window_Form_Value = GetDictValue(dict, nameof(Settings_Domain_Window_Form_Value), Settings_Domain_Window_Form_Value);
            Settings_Domain_Window_Form_Server = GetDictValue(dict, nameof(Settings_Domain_Window_Form_Server), Settings_Domain_Window_Form_Server);
            Settings_Domain_Window_Form_Match = GetDictValue(dict, nameof(Settings_Domain_Window_Form_Match), Settings_Domain_Window_Form_Match);
            Settings_Domain_Window_Form_Enable = GetDictValue(dict, nameof(Settings_Domain_Window_Form_Enable), Settings_Domain_Window_Form_Enable);
            Settings_Domain_Window_Button_Save = GetDictValue(dict, nameof(Settings_Domain_Window_Button_Save), Settings_Domain_Window_Button_Save);
            Settings_Domain_Window_Button_Cancel = GetDictValue(dict, nameof(Settings_Domain_Window_Button_Cancel), Settings_Domain_Window_Button_Cancel);
            Settings_Domain_Window_Error_Domain_Empty = GetDictValue(dict, nameof(Settings_Domain_Window_Error_Domain_Empty), Settings_Domain_Window_Error_Domain_Empty);
            Settings_Domain_Window_Error_IPv4_Invalid = GetDictValue(dict, nameof(Settings_Domain_Window_Error_IPv4_Invalid), Settings_Domain_Window_Error_IPv4_Invalid);
            Settings_Domain_Window_Error_Record_Invalid = GetDictValue(dict, nameof(Settings_Domain_Window_Error_Record_Invalid), Settings_Domain_Window_Error_Record_Invalid);
            Settings_Domain_Window_Error_Exists = GetDictValue(dict, nameof(Settings_Domain_Window_Error_Exists), Settings_Domain_Window_Error_Exists);

            // 遠端規則
            Settings_Remote_Tooltip_Edit = GetDictValue(dict, nameof(Settings_Remote_Tooltip_Edit), Settings_Remote_Tooltip_Edit);
            Settings_Remote_Tooltip_Delete = GetDictValue(dict, nameof(Settings_Remote_Tooltip_Delete), Settings_Remote_Tooltip_Delete);
            Settings_Remote_Unit_Minute = GetDictValue(dict, nameof(Settings_Remote_Unit_Minute), Settings_Remote_Unit_Minute);
            Settings_Remote_Confirm_Delete = GetDictValue(dict, nameof(Settings_Remote_Confirm_Delete), Settings_Remote_Confirm_Delete);
            Settings_Remote_Window_Add_Title = GetDictValue(dict, nameof(Settings_Remote_Window_Add_Title), Settings_Remote_Window_Add_Title);
            Settings_Remote_Window_Edit_Title = GetDictValue(dict, nameof(Settings_Remote_Window_Edit_Title), Settings_Remote_Window_Edit_Title);
            Settings_Remote_Window_Form_Name = GetDictValue(dict, nameof(Settings_Remote_Window_Form_Name), Settings_Remote_Window_Form_Name);
            Settings_Remote_Window_Form_URL = GetDictValue(dict, nameof(Settings_Remote_Window_Form_URL), Settings_Remote_Window_Form_URL);
            Settings_Remote_Window_Form_Interval = GetDictValue(dict, nameof(Settings_Remote_Window_Form_Interval), Settings_Remote_Window_Form_Interval);
            Settings_Remote_Window_Form_Enable = GetDictValue(dict, nameof(Settings_Remote_Window_Form_Enable), Settings_Remote_Window_Form_Enable);
            Settings_Remote_Window_Button_Save = GetDictValue(dict, nameof(Settings_Remote_Window_Button_Save), Settings_Remote_Window_Button_Save);
            Settings_Remote_Window_Button_Cancel = GetDictValue(dict, nameof(Settings_Remote_Window_Button_Cancel), Settings_Remote_Window_Button_Cancel);
            Settings_Remote_Window_Error_Name_Empty = GetDictValue(dict, nameof(Settings_Remote_Window_Error_Name_Empty), Settings_Remote_Window_Error_Name_Empty);
            Settings_Remote_Window_Error_Url_Invalid = GetDictValue(dict, nameof(Settings_Remote_Window_Error_Url_Invalid), Settings_Remote_Window_Error_Url_Invalid);
            Settings_Remote_Window_Error_Interval_Invalid = GetDictValue(dict, nameof(Settings_Remote_Window_Error_Interval_Invalid), Settings_Remote_Window_Error_Interval_Invalid);
            Settings_Remote_Window_Error_Name_Exists = GetDictValue(dict, nameof(Settings_Remote_Window_Error_Name_Exists), Settings_Remote_Window_Error_Name_Exists);

            // 系統設定
            Settings_System_Language = GetDictValue(dict, nameof(Settings_System_Language), Settings_System_Language);
            Settings_Button_Service_Save = GetDictValue(dict, nameof(Settings_Button_Service_Save), Settings_Button_Service_Save);
            Settings_Button_Service_Add = GetDictValue(dict, nameof(Settings_Button_Service_Add), Settings_Button_Service_Add);
            Settings_Button_Remote_Add = GetDictValue(dict, nameof(Settings_Button_Remote_Add), Settings_Button_Remote_Add);
            Settings_Button_System_Save = GetDictValue(dict, nameof(Settings_Button_System_Save), Settings_Button_System_Save);
            Settings_Error_Save_Fail = GetDictValue(dict, nameof(Settings_Error_Save_Fail), Settings_Error_Save_Fail);
            Settings_Save_Success = GetDictValue(dict, nameof(Settings_Save_Success), Settings_Save_Success);

            // 確認對話框
            Confirm_Default_Title = GetDictValue(dict, nameof(Confirm_Default_Title), Confirm_Default_Title);
            Confirm_Button_OK = GetDictValue(dict, nameof(Confirm_Button_OK), Confirm_Button_OK);
            Confirm_Button_Cancel = GetDictValue(dict, nameof(Confirm_Button_Cancel), Confirm_Button_Cancel);
            Confirm_Exit_Message = GetDictValue(dict, nameof(Confirm_Exit_Message), Confirm_Exit_Message);
            Confirm_Stop_Message = GetDictValue(dict, nameof(Confirm_Stop_Message), Confirm_Stop_Message);
            Confirm_Info_Title = GetDictValue(dict, nameof(Confirm_Info_Title), Confirm_Info_Title);
            Confirm_Error_Title = GetDictValue(dict, nameof(Confirm_Error_Title), Confirm_Error_Title);

            // 關於頁面
            About_Homepage = GetDictValue(dict, nameof(About_Homepage), About_Homepage);
            About_Version = GetDictValue(dict, nameof(About_Version), About_Version);
            About_Thank = GetDictValue(dict, nameof(About_Thank), About_Thank);

            // 語言標識
            Language_Name = GetDictValue(dict, nameof(Language_Name), Language_Name);
        }

        private void OnPropertyChanged(System.Collections.Generic.Dictionary<string, string> dict)
        {
            foreach(KeyValuePair<string, string> item in dict)
            {
                base.OnPropertyChanged(item.Key);
            }
        }
        public void Change(string language)
        {
            Dictionary<string,string>? langPackage= null;
            switch (language) {
                case "zh-CN":
                    {
                        langPackage = zhCN_Dict;
                        
                        break;
                    }
                case "en-US":
                    {
                        langPackage = enUS_Dict;
                        break;
                    }
            }
            if (langPackage != null) {
                useDict(langPackage);
                OnPropertyChanged(langPackage);
            }
            // 从文件加载语言
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = System.IO.Path.Combine(path, "locales");
            path = System.IO.Path.Combine(path, language+".lang");
            try
            {
                langPackage = LoadLanguage(path);
                useDict(langPackage);
                OnPropertyChanged(langPackage);
            }
            catch (Exception)
            {

            }
            finally { 
            
            }
        }
        
        private static System.Collections.Generic.Dictionary<string,string> LoadLanguage(string file,string? endKey=null)
        {
            
            string[] lines=File.ReadAllLines(file);
            System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();
            for (int i = 0; i < lines.Length; i++) {
                string line = lines[i].Trim();

                // 跳过空行和注释行
                if (string.IsNullOrEmpty(line) || line.StartsWith("#") || line.StartsWith("//"))
                {
                    continue;
                }

                // 分割键值对
                int equalIndex = line.IndexOf('=');
                if (equalIndex == -1)
                {
                    continue; // 如果没有等号，跳过这一行
                }

                string key = line.Substring(0, equalIndex).Trim();
                string value = line.Substring(equalIndex + 1).Trim();

                // 处理引号中的值
                if (value.StartsWith("\"")|| value.StartsWith("\'"))
                {
                    //&& value.EndsWith("\"")
                    char qc = value[0];
                    int findPos=1;
                    while (true) {
                        int quote = value.IndexOf(qc, findPos);
                        if (quote == -1) {
                            // 错误的配置
                            value = value.Substring(1);
                            break;
                        }
                        if ((value[quote-1]== qc))
                        {
                            findPos = quote + 1;
                        }
                        else
                        {
                            // 配置结束
                            value = value.Substring(1, quote - 1);
                            break;
                        }
                    }
                    
                    //value = value.Substring(1, value.Length - 2);
                    value = value.Replace("\\"+qc, qc+""); // 处理转义的双引号
                }
                else
                {
                    //从#或//开始移除
                    int t = value.IndexOf('#');
                    if (t != -1) {
                        value = value.Substring(0, t);
                    }
                    t = value.IndexOf("//");
                    if (t != -1)
                    {
                        value = value.Substring(0, t);
                    }
                }

                // 将键值对存入字典
                dict[key] = value;
                if (endKey!=null&&key == endKey) break;
            }
            return dict;
        }
        public static Language[] GetLanguages()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = System.IO.Path.Combine(path, "locales");
            string[] files = [];
            try
            {
                files=System.IO.Directory.GetFiles(path, "*.lang");
            }
            catch (Exception)
            {

            }
            List<Language> languages = new List<Language>();

            languages.Add(new Language() { Code = "en-US", Name = "English" });
            languages.Add(new Language() { Code = "zh-CN", Name = "简体中文" });
            for (int i= 0;i<files.Length;i++)
            {
                string file=files[i];
                string filePath= System.IO.Path.Combine(path, file);
                System.Collections.Generic.Dictionary<string, string> language_kv = LoadLanguage(filePath, "Language_Name");
                string name = "";
                if (language_kv.ContainsKey("Language_Name"))
                {
                    name = language_kv["Language_Name"];
                }
                string code = System.IO.Path.GetFileNameWithoutExtension(filePath);
                if(code!= "en-US" && code != "zh-CN")
                {
                    languages.Add(new Language() { Code = code, Name = name });
                }
                

            }
            return languages.ToArray();
        }
    }
    internal class I18n<T> : ViewModelBase
    {
        public I18n i18n => I18n.i18n;
        public T Value { get; private set; }
        public I18n(T v){
            this.Value = v;
        }
            
    }

    internal class Language
    {
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
    }
}
