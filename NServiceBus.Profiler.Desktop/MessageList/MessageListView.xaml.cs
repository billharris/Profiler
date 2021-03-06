﻿using System.Linq;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Grid;
using NServiceBus.Profiler.Common.Models;
using NServiceBus.Profiler.Desktop.Shell;

namespace NServiceBus.Profiler.Desktop.MessageList
{
    /// <summary>
    /// Interaction logic for MessageListView.xaml
    /// </summary>
    public partial class MessageListView 
    {
        private readonly IShellViewModel _shell;
        private readonly IMenuManager _menuManager;
        private PopupMenu _contextMenu;

        public MessageListView()
        {
            InitializeComponent();
        }

        public MessageListView(IShellViewModel shell, IMenuManager menuManager)
            : this()
        {
            _shell = shell;
            _menuManager = menuManager;
            InitializeContextMenu();
        }

        private void InitializeContextMenu()
        {
            _contextMenu = _menuManager.CreateContextMenu(grid.View);
            _contextMenu.Opening += (s, e) => OnPluginContextMenuOpening();
        }

        private void OnPluginContextMenuOpening()
        {
            _contextMenu.ItemLinks.Clear();

            //TODO: How to populate contextmenus
//            var menuItems = _shell.Plugins.SelectMany(plugin => plugin.ContextMenuItems)
//                                          .OrderBy(mi => mi.Order).ToList();
//
//            menuItems.ForEach(mi =>
//            {
//                var menu = _menuManager.CreateContextMenuItem(mi);
//                _contextMenu.ItemLinks.Add(menu);
//            });
        }

        private void OnFocusedMessageChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (Model != null)
            {
                Model.FocusedMessage = e.NewRow as MessageInfo;
            }
        }

        private IMessageListViewModel Model
        {
            get { return (IMessageListViewModel)DataContext; }
        }

        private void OnSelectedMessagesChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if (Model != null)
            {
                Model.SelectedMessages.Clear();

                foreach (var row in e.Source.SelectedRows)
                {
                    Model.SelectedMessages.Add((MessageInfo) row); 
                }
            }
        }
    }
}
