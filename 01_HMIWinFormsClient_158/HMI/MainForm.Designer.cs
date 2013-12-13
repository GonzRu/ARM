namespace HMI
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.навигацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.сменаПользователяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.блокировкаСистемыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.информацияОПользователеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.навигацияToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.мнемосхемаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.быстрыйДоступToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ведомостиИЖурналыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.действияПользователейToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.событияОКУИРЗАToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.журналАварийИОсщиллограммToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.текущийОтчетToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.инструментыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.сигнализацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.цветФонаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripTextBoxColor = new System.Windows.Forms.ToolStripTextBox();
			this.администрированиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.управлениеДоступомToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.базаДанныхToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.созданиеРасписанияРезервногоКопированияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.восстановлениеБазыДанныхСДХToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.установкаВремениToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.настройкаСообщенийПТКToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.окнаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.упорядочитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.каскадомToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.поВертикалиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.поГоризонталиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.hugeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.normalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tinyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.sbMesIE = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbConnectBD = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbConnectFC = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbCountEvent = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbStatusCurrentOperation = new System.Windows.Forms.ToolStripProgressBar();
			this.sbDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.miToolStrip_currentData = new System.Windows.Forms.ToolStripMenuItem();
			this.miToolStrip_currentTime = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripStatusLabelClock = new System.Windows.Forms.ToolStripStatusLabel();
			this.timerDataTimeUpdate = new System.Windows.Forms.Timer(this.components);
			this.menuStrip1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.навигацияToolStripMenuItem,
            this.навигацияToolStripMenuItem1,
            this.ведомостиИЖурналыToolStripMenuItem,
            this.инструментыToolStripMenuItem,
            this.администрированиеToolStripMenuItem,
            this.окнаToolStripMenuItem,
            this.справкаToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.MdiWindowListItem = this.окнаToolStripMenuItem;
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1016, 24);
			this.menuStrip1.TabIndex = 31;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// навигацияToolStripMenuItem
			// 
			this.навигацияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.сменаПользователяToolStripMenuItem,
            this.блокировкаСистемыToolStripMenuItem,
            this.информацияОПользователеToolStripMenuItem,
            this.toolStripSeparator2,
            this.выходToolStripMenuItem});
			this.навигацияToolStripMenuItem.Name = "навигацияToolStripMenuItem";
			this.навигацияToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.навигацияToolStripMenuItem.Text = "Система";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(228, 6);
			// 
			// сменаПользователяToolStripMenuItem
			// 
			this.сменаПользователяToolStripMenuItem.Image = global::HMI.Properties.Resources.sysenter;
			this.сменаПользователяToolStripMenuItem.Name = "сменаПользователяToolStripMenuItem";
			this.сменаПользователяToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
			this.сменаПользователяToolStripMenuItem.Text = "Смена пользователя";
			// 
			// блокировкаСистемыToolStripMenuItem
			// 
			this.блокировкаСистемыToolStripMenuItem.Image = global::HMI.Properties.Resources.syslock;
			this.блокировкаСистемыToolStripMenuItem.Name = "блокировкаСистемыToolStripMenuItem";
			this.блокировкаСистемыToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
			this.блокировкаСистемыToolStripMenuItem.Text = "Блокировка системы";
			// 
			// информацияОПользователеToolStripMenuItem
			// 
			this.информацияОПользователеToolStripMenuItem.Image = global::HMI.Properties.Resources.user;
			this.информацияОПользователеToolStripMenuItem.Name = "информацияОПользователеToolStripMenuItem";
			this.информацияОПользователеToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
			this.информацияОПользователеToolStripMenuItem.Text = "Информация о пользователе";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(228, 6);
			// 
			// выходToolStripMenuItem
			// 
			this.выходToolStripMenuItem.Image = global::HMI.Properties.Resources.sysleave;
			this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
			this.выходToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
			this.выходToolStripMenuItem.Text = "Выход";
			this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
			// 
			// навигацияToolStripMenuItem1
			// 
			this.навигацияToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.мнемосхемаToolStripMenuItem,
            this.быстрыйДоступToolStripMenuItem});
			this.навигацияToolStripMenuItem1.Name = "навигацияToolStripMenuItem1";
			this.навигацияToolStripMenuItem1.Size = new System.Drawing.Size(73, 20);
			this.навигацияToolStripMenuItem1.Text = "Навигация";
			// 
			// мнемосхемаToolStripMenuItem
			// 
			this.мнемосхемаToolStripMenuItem.Image = global::HMI.Properties.Resources.mnemo_pict2;
			this.мнемосхемаToolStripMenuItem.Name = "мнемосхемаToolStripMenuItem";
			this.мнемосхемаToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.мнемосхемаToolStripMenuItem.Text = "Мнемосхема";
			this.мнемосхемаToolStripMenuItem.Click += new System.EventHandler(this.мнемосхемаToolStripMenuItem_Click);
			// 
			// быстрыйДоступToolStripMenuItem
			// 
			this.быстрыйДоступToolStripMenuItem.Name = "быстрыйДоступToolStripMenuItem";
			this.быстрыйДоступToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.быстрыйДоступToolStripMenuItem.Text = "Быстрый доступ";
			// 
			// ведомостиИЖурналыToolStripMenuItem
			// 
			this.ведомостиИЖурналыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.действияПользователейToolStripMenuItem,
            this.событияОКУИРЗАToolStripMenuItem,
            this.журналАварийИОсщиллограммToolStripMenuItem,
            this.текущийОтчетToolStripMenuItem});
			this.ведомостиИЖурналыToolStripMenuItem.Name = "ведомостиИЖурналыToolStripMenuItem";
			this.ведомостиИЖурналыToolStripMenuItem.Size = new System.Drawing.Size(131, 20);
			this.ведомостиИЖурналыToolStripMenuItem.Text = "Ведомости и журналы";
			// 
			// действияПользователейToolStripMenuItem
			// 
			this.действияПользователейToolStripMenuItem.Name = "действияПользователейToolStripMenuItem";
			this.действияПользователейToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
			this.действияПользователейToolStripMenuItem.Text = "Действия пользователей";
			// 
			// событияОКУИРЗАToolStripMenuItem
			// 
			this.событияОКУИРЗАToolStripMenuItem.Name = "событияОКУИРЗАToolStripMenuItem";
			this.событияОКУИРЗАToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
			this.событияОКУИРЗАToolStripMenuItem.Text = "События ОКУ и РЗА";
			// 
			// журналАварийИОсщиллограммToolStripMenuItem
			// 
			this.журналАварийИОсщиллограммToolStripMenuItem.Name = "журналАварийИОсщиллограммToolStripMenuItem";
			this.журналАварийИОсщиллограммToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
			this.журналАварийИОсщиллограммToolStripMenuItem.Text = "Журнал аварий и осщиллограмм";
			// 
			// текущийОтчетToolStripMenuItem
			// 
			this.текущийОтчетToolStripMenuItem.Name = "текущийОтчетToolStripMenuItem";
			this.текущийОтчетToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
			this.текущийОтчетToolStripMenuItem.Text = "Текущий отчет";
			// 
			// инструментыToolStripMenuItem
			// 
			this.инструментыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сигнализацияToolStripMenuItem,
            this.настройкиToolStripMenuItem});
			this.инструментыToolStripMenuItem.Image = global::HMI.Properties.Resources.setup;
			this.инструментыToolStripMenuItem.Name = "инструментыToolStripMenuItem";
			this.инструментыToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
			this.инструментыToolStripMenuItem.Text = "Настройки";
			// 
			// сигнализацияToolStripMenuItem
			// 
			this.сигнализацияToolStripMenuItem.Image = global::HMI.Properties.Resources.alarm;
			this.сигнализацияToolStripMenuItem.Name = "сигнализацияToolStripMenuItem";
			this.сигнализацияToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.сигнализацияToolStripMenuItem.Text = "Сигнализация";
			// 
			// настройкиToolStripMenuItem
			// 
			this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.цветФонаToolStripMenuItem});
			this.настройкиToolStripMenuItem.Image = global::HMI.Properties.Resources.gears;
			this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
			this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.настройкиToolStripMenuItem.Text = "Опции";
			// 
			// цветФонаToolStripMenuItem
			// 
			this.цветФонаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBoxColor});
			this.цветФонаToolStripMenuItem.Name = "цветФонаToolStripMenuItem";
			this.цветФонаToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
			this.цветФонаToolStripMenuItem.Text = "Цвет фона";
			// 
			// toolStripTextBoxColor
			// 
			this.toolStripTextBoxColor.Name = "toolStripTextBoxColor";
			this.toolStripTextBoxColor.Size = new System.Drawing.Size(100, 21);
			// 
			// администрированиеToolStripMenuItem
			// 
			this.администрированиеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.управлениеДоступомToolStripMenuItem,
            this.базаДанныхToolStripMenuItem,
            this.установкаВремениToolStripMenuItem,
            this.настройкаСообщенийПТКToolStripMenuItem});
			this.администрированиеToolStripMenuItem.Name = "администрированиеToolStripMenuItem";
			this.администрированиеToolStripMenuItem.Size = new System.Drawing.Size(122, 20);
			this.администрированиеToolStripMenuItem.Text = "Администрирование";
			// 
			// управлениеДоступомToolStripMenuItem
			// 
			this.управлениеДоступомToolStripMenuItem.Name = "управлениеДоступомToolStripMenuItem";
			this.управлениеДоступомToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
			this.управлениеДоступомToolStripMenuItem.Text = "Управление доступом";
			// 
			// базаДанныхToolStripMenuItem
			// 
			this.базаДанныхToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.созданиеРасписанияРезервногоКопированияToolStripMenuItem,
            this.восстановлениеБазыДанныхСДХToolStripMenuItem});
			this.базаДанныхToolStripMenuItem.Image = global::HMI.Properties.Resources.database;
			this.базаДанныхToolStripMenuItem.Name = "базаДанныхToolStripMenuItem";
			this.базаДанныхToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
			this.базаДанныхToolStripMenuItem.Text = "Управление СДХ";
			// 
			// созданиеРасписанияРезервногоКопированияToolStripMenuItem
			// 
			this.созданиеРасписанияРезервногоКопированияToolStripMenuItem.Name = "созданиеРасписанияРезервногоКопированияToolStripMenuItem";
			this.созданиеРасписанияРезервногоКопированияToolStripMenuItem.Size = new System.Drawing.Size(320, 22);
			this.созданиеРасписанияРезервногоКопированияToolStripMenuItem.Text = "Управление резервным копированием БД СДХ";
			// 
			// восстановлениеБазыДанныхСДХToolStripMenuItem
			// 
			this.восстановлениеБазыДанныхСДХToolStripMenuItem.Name = "восстановлениеБазыДанныхСДХToolStripMenuItem";
			this.восстановлениеБазыДанныхСДХToolStripMenuItem.Size = new System.Drawing.Size(320, 22);
			this.восстановлениеБазыДанныхСДХToolStripMenuItem.Text = "Восстановление базы данных СДХ";
			// 
			// установкаВремениToolStripMenuItem
			// 
			this.установкаВремениToolStripMenuItem.Image = global::HMI.Properties.Resources.clock;
			this.установкаВремениToolStripMenuItem.Name = "установкаВремениToolStripMenuItem";
			this.установкаВремениToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
			this.установкаВремениToolStripMenuItem.Text = "Установка времени";
			// 
			// настройкаСообщенийПТКToolStripMenuItem
			// 
			this.настройкаСообщенийПТКToolStripMenuItem.Name = "настройкаСообщенийПТКToolStripMenuItem";
			this.настройкаСообщенийПТКToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
			this.настройкаСообщенийПТКToolStripMenuItem.Text = "Настройка сообщений ПТК";
			// 
			// окнаToolStripMenuItem
			// 
			this.окнаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.упорядочитьToolStripMenuItem});
			this.окнаToolStripMenuItem.Name = "окнаToolStripMenuItem";
			this.окнаToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
			this.окнаToolStripMenuItem.Text = "Окна";
			// 
			// упорядочитьToolStripMenuItem
			// 
			this.упорядочитьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.каскадомToolStripMenuItem,
            this.поВертикалиToolStripMenuItem,
            this.поГоризонталиToolStripMenuItem});
			this.упорядочитьToolStripMenuItem.Name = "упорядочитьToolStripMenuItem";
			this.упорядочитьToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
			this.упорядочитьToolStripMenuItem.Text = "Упорядочить";
			// 
			// каскадомToolStripMenuItem
			// 
			this.каскадомToolStripMenuItem.Name = "каскадомToolStripMenuItem";
			this.каскадомToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.каскадомToolStripMenuItem.Text = "Каскадом";
			this.каскадомToolStripMenuItem.Click += new System.EventHandler(this.каскадомToolStripMenuItem_Click);
			// 
			// поВертикалиToolStripMenuItem
			// 
			this.поВертикалиToolStripMenuItem.Name = "поВертикалиToolStripMenuItem";
			this.поВертикалиToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.поВертикалиToolStripMenuItem.Text = "По вертикали";
			this.поВертикалиToolStripMenuItem.Click += new System.EventHandler(this.поВертикалиToolStripMenuItem_Click);
			// 
			// поГоризонталиToolStripMenuItem
			// 
			this.поГоризонталиToolStripMenuItem.Name = "поГоризонталиToolStripMenuItem";
			this.поГоризонталиToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.поГоризонталиToolStripMenuItem.Text = "По горизонтали";
			this.поГоризонталиToolStripMenuItem.Click += new System.EventHandler(this.поГоризонталиToolStripMenuItem_Click);
			// 
			// справкаToolStripMenuItem
			// 
			this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem,
            this.помощьToolStripMenuItem});
			this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
			this.справкаToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
			this.справкаToolStripMenuItem.Text = "Справка";
			// 
			// оПрограммеToolStripMenuItem
			// 
			this.оПрограммеToolStripMenuItem.Image = global::HMI.Properties.Resources.Logo;
			this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
			this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.оПрограммеToolStripMenuItem.Text = "О программе";
			// 
			// помощьToolStripMenuItem
			// 
			this.помощьToolStripMenuItem.Image = global::HMI.Properties.Resources.userinfo;
			this.помощьToolStripMenuItem.Name = "помощьToolStripMenuItem";
			this.помощьToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.помощьToolStripMenuItem.Text = "Помощь";
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hugeToolStripMenuItem,
            this.normalToolStripMenuItem,
            this.tinyToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(134, 70);
			// 
			// hugeToolStripMenuItem
			// 
			this.hugeToolStripMenuItem.Name = "hugeToolStripMenuItem";
			this.hugeToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
			this.hugeToolStripMenuItem.Text = "Крупный";
			this.hugeToolStripMenuItem.Click += new System.EventHandler(this.крупныйToolStripMenuItem_Click);
			// 
			// normalToolStripMenuItem
			// 
			this.normalToolStripMenuItem.Name = "normalToolStripMenuItem";
			this.normalToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
			this.normalToolStripMenuItem.Text = "Обычный";
			this.normalToolStripMenuItem.Click += new System.EventHandler(this.крупныйToolStripMenuItem_Click);
			// 
			// tinyToolStripMenuItem
			// 
			this.tinyToolStripMenuItem.Name = "tinyToolStripMenuItem";
			this.tinyToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
			this.tinyToolStripMenuItem.Text = "Мелкий";
			this.tinyToolStripMenuItem.Click += new System.EventHandler(this.крупныйToolStripMenuItem_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sbMesIE,
            this.sbConnectBD,
            this.sbConnectFC,
            this.sbCountEvent,
            this.sbStatusCurrentOperation,
            this.sbDropDownButton1,
            this.toolStripStatusLabelClock});
			this.statusStrip1.Location = new System.Drawing.Point(0, 712);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1016, 22);
			this.statusStrip1.TabIndex = 32;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// sbMesIE
			// 
			this.sbMesIE.BackColor = System.Drawing.SystemColors.Info;
			this.sbMesIE.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
							| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
							| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.sbMesIE.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.sbMesIE.Name = "sbMesIE";
			this.sbMesIE.Size = new System.Drawing.Size(193, 17);
			this.sbMesIE.Spring = true;
			this.sbMesIE.Text = "ПТК \"Защита\"";
			// 
			// sbConnectBD
			// 
			this.sbConnectBD.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
							| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
							| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.sbConnectBD.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.sbConnectBD.Name = "sbConnectBD";
			this.sbConnectBD.Size = new System.Drawing.Size(193, 17);
			this.sbConnectBD.Spring = true;
			this.sbConnectBD.Text = "Связь с БД";
			// 
			// sbConnectFC
			// 
			this.sbConnectFC.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
							| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
							| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.sbConnectFC.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.sbConnectFC.Name = "sbConnectFC";
			this.sbConnectFC.Size = new System.Drawing.Size(193, 17);
			this.sbConnectFC.Spring = true;
			this.sbConnectFC.Text = "Связь с ФК";
			// 
			// sbCountEvent
			// 
			this.sbCountEvent.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
							| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
							| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.sbCountEvent.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.sbCountEvent.Name = "sbCountEvent";
			this.sbCountEvent.Size = new System.Drawing.Size(193, 17);
			this.sbCountEvent.Spring = true;
			this.sbCountEvent.Text = "Количество событий: (A)-  ; (I)  ;";
			// 
			// sbStatusCurrentOperation
			// 
			this.sbStatusCurrentOperation.Name = "sbStatusCurrentOperation";
			this.sbStatusCurrentOperation.Size = new System.Drawing.Size(100, 16);
			this.sbStatusCurrentOperation.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.sbStatusCurrentOperation.ToolTipText = "Состояние текущей операции";
			// 
			// sbDropDownButton1
			// 
			this.sbDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.sbDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miToolStrip_currentData,
            this.miToolStrip_currentTime});
			this.sbDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("sbDropDownButton1.Image")));
			this.sbDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.sbDropDownButton1.Name = "sbDropDownButton1";
			this.sbDropDownButton1.Size = new System.Drawing.Size(29, 20);
			this.sbDropDownButton1.Text = "toolStripDropDownButton1";
			// 
			// miToolStrip_currentData
			// 
			this.miToolStrip_currentData.Name = "miToolStrip_currentData";
			this.miToolStrip_currentData.Size = new System.Drawing.Size(163, 22);
			this.miToolStrip_currentData.Text = "Текущая дата";
			this.miToolStrip_currentData.Click += new System.EventHandler(this.miToolStrip_currentData_Click);
			// 
			// miToolStrip_currentTime
			// 
			this.miToolStrip_currentTime.Name = "miToolStrip_currentTime";
			this.miToolStrip_currentTime.Size = new System.Drawing.Size(163, 22);
			this.miToolStrip_currentTime.Text = "Текущее время";
			this.miToolStrip_currentTime.Click += new System.EventHandler(this.miToolStrip_currentTime_Click);
			// 
			// toolStripStatusLabelClock
			// 
			this.toolStripStatusLabelClock.Name = "toolStripStatusLabelClock";
			this.toolStripStatusLabelClock.Size = new System.Drawing.Size(67, 17);
			this.toolStripStatusLabelClock.Text = "Время/Дата";
			// 
			// timerDataTimeUpdate
			// 
			this.timerDataTimeUpdate.Enabled = true;
			this.timerDataTimeUpdate.Interval = 1000;
			this.timerDataTimeUpdate.Tick += new System.EventHandler(this.timerDataTimeUpdate_Tick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.ClientSize = new System.Drawing.Size(1016, 734);
			this.ContextMenuStrip = this.contextMenuStrip1;
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.IsMdiContainer = true;
			this.Name = "MainForm";
			this.Text = "САУ ЗРУ 10кВ КС \"Валдай\" (НТЦ \"Механотроника\")";
			this.TopMost = true;
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
			this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem навигацияToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem инструментыToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem цветФонаToolStripMenuItem;
		private System.Windows.Forms.ToolStripTextBox toolStripTextBoxColor;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem hugeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem normalToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tinyToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem сменаПользователяToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem блокировкаСистемыToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem информацияОПользователеToolStripMenuItem;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
      private System.Windows.Forms.ToolStripMenuItem навигацияToolStripMenuItem1;
      private System.Windows.Forms.ToolStripMenuItem мнемосхемаToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem быстрыйДоступToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem ведомостиИЖурналыToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem действияПользователейToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem событияОКУИРЗАToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem журналАварийИОсщиллограммToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem администрированиеToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem текущийОтчетToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem сигнализацияToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem управлениеДоступомToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem базаДанныхToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem созданиеРасписанияРезервногоКопированияToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem восстановлениеБазыДанныхСДХToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem установкаВремениToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem настройкаСообщенийПТКToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem помощьToolStripMenuItem;
      private System.Windows.Forms.StatusStrip statusStrip1;
      private System.Windows.Forms.ToolStripStatusLabel sbMesIE;
      private System.Windows.Forms.ToolStripStatusLabel sbConnectBD;
      private System.Windows.Forms.ToolStripStatusLabel sbConnectFC;
      private System.Windows.Forms.ToolStripStatusLabel sbCountEvent;
      private System.Windows.Forms.ToolStripProgressBar sbStatusCurrentOperation;
		private System.Windows.Forms.ToolStripDropDownButton sbDropDownButton1;
		private System.Windows.Forms.ToolStripMenuItem miToolStrip_currentData;
		private System.Windows.Forms.ToolStripMenuItem miToolStrip_currentTime;
		private System.Windows.Forms.Timer timerDataTimeUpdate;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelClock;
		private System.Windows.Forms.ToolStripMenuItem окнаToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem упорядочитьToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem каскадомToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem поВертикалиToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem поГоризонталиToolStripMenuItem;
		





   }
}

