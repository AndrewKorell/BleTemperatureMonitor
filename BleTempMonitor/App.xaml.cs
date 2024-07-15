﻿using BleTempMonitor.Services;
using System.Security.Cryptography.X509Certificates;

namespace BleTempMonitor
{
    public partial class App : Application
    {

        private static IServiceProvider ServicesProvider;
        public static IServiceProvider Services => ServicesProvider;

        private static IAlertService AlertService;
        public static IAlertService AlertSvc => AlertService;

        public readonly static LogService Logger = new();

        public App(IServiceProvider provider)
        {
            InitializeComponent();

            ServicesProvider = provider;
            AlertService = Services.GetService<IAlertService>();

            MainPage = new AppShell();
        }
    }
}
