﻿using System.Collections.Generic;
using Caliburn.PresentationFramework.Filters;
using Caliburn.PresentationFramework.Screens;
using NServiceBus.Profiler.Common.ExtensionMethods;
using NServiceBus.Profiler.Common.Models;
using NServiceBus.Profiler.Core;
using NServiceBus.Profiler.Desktop.Explorer;

namespace NServiceBus.Profiler.Desktop.Shell
{
    public class QueueCreationViewModel : Screen
    {
        private readonly IQueueManager _queueManager;
        private readonly IExplorerViewModel _explorer;
        private readonly INetworkOperations _networkOperations;

        public QueueCreationViewModel(
            IQueueManager queueManager, 
            IExplorerViewModel explorer,
            INetworkOperations networkOperations)
        {
            _queueManager = queueManager;
            _explorer = explorer;
            _networkOperations = networkOperations;
            Machines = new List<string>();
            DisplayName = "Queue";
            IsTransactional = true;
        }

        public virtual string QueueName { get; set; }

        public virtual string SelectedMachine { get; set; }

        public virtual bool IsTransactional { get; set; }

        public virtual List<string> Machines { get; private set; }

        protected override void OnActivate()
        {
            base.OnActivate();

            Machines.Clear();
            Machines.AddRange(_networkOperations.GetMachines());
            SelectedMachine = _explorer.ConnectedToComputer;
        }

        public virtual void Close()
        {
            TryClose(false);
        }

        public virtual bool CanAccept()
        {
            return !QueueName.IsEmpty() &&
                   !SelectedMachine.IsEmpty();
        }

        [AutoCheckAvailability]
        public virtual void Accept()
        {
            if (CreateQueue())
            {
                TryClose(true);
            }
        }

        private bool CreateQueue()
        {
            var queue = _queueManager.CreatePrivateQueue(new Queue(SelectedMachine, QueueName), IsTransactional);
            return queue != null;
        }
    }
}