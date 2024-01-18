﻿using BORGWARNER_SERVOPRESS.DataModel.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views
{
    public class ViewMain
    {
        private ModelViewMain _modelViewMain;
        public ViewMain()
        {
            _modelViewMain = new ModelViewMain();
        }
        public ModelViewMain GetModel()
        {
            return _modelViewMain;
        }
        public void ShowDate()
        {
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, args) =>
            {
                _modelViewMain.Timestamp = DateTime.Now.ToString();   
                _modelViewMain.NumeroAleatorio = new Random().Next(1, 1000000);
            };
            timer.Start();
        }
        public async Task getStatusScrew(string messageScrew)
        {            
            _modelViewMain.MessageScrew = messageScrew;                                
        }
    }
}
