﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invert.Data;
using Invert.IOC;

namespace Invert.Core.GraphDesigner
{
    public class TypesSystem : DiagramPlugin
        , IContextMenuQuery
        , IExecuteCommand<SelectTypeCommand>
    {
        public override void Loaded(UFrameContainer container)
        {
            base.Loaded(container);
            TypesInfo = InvertGraphEditor.TypesContainer.ResolveAll<GraphTypeInfo>().ToArray();
            Repository = container.Resolve<IRepository>();
        }

        public IRepository Repository { get; set; }

        public GraphTypeInfo[] TypesInfo { get; set; }

        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, object obj)
        {
            var typedItem = obj as TypedItemViewModel;
            
            if (typedItem != null)
            {
                foreach (var item in TypesInfo)
                {
                    var item1 = item;
                    ui.AddCommand(new ContextMenuItem()
                    {
                        Title = item1.Name,
                        Group = item.Group,
                        Command = new LambdaCommand("Change Type", () =>
                        {
                            typedItem.RelatedType = item1.Name;
                        })
                    });
                }
            }
            var nodeItem = obj as ItemViewModel;
            if (nodeItem != null)
            {
                ui.AddCommand(new ContextMenuItem()
                {
                    Title = "Delete",
                    Command = new DeleteCommand()
                    {
                        Title = "Delete Item",
                        Item = nodeItem.DataObject as IDataRecord
                    }
                });
            }
        }


        public void Execute(SelectTypeCommand command)
        {
            
            var menu = new SelectionMenu();
            var types = GetRelatedTypes(command).ToArray();

            if (command.AllowNone)
            {
                menu.AddItem(new SelectionMenuItem("", "None", () =>
                {
                    command.ItemViewModel.RelatedType = null;
                }));
            }

            var categories = types.Where(_=>!string.IsNullOrEmpty(_.Group)).Select(_ => _.Group).Distinct().Select(_ => new SelectionMenuCategory()
            {
                Title = _
            });

            foreach (var category in categories)
            {
                menu.AddItem(category);
                var category1 = category;
                foreach (var type in types.Where(_=>_.Group == category1.Title))
                {
                    var type1 = type;
                    menu.AddItem(new SelectionMenuItem(type, () =>
                    {
                        command.ItemViewModel.RelatedType = type1.Name;
                    }),category);
                }
            }

            foreach (var source in types.Where(_=>string.IsNullOrEmpty(_.Group)))
            {
                var type1 = source;
                menu.AddItem(new SelectionMenuItem(type1, () =>
                {
                    command.ItemViewModel.RelatedType = type1.Name;
                }));
            }

            Signal<IShowSelectionMenu>(_=>_.ShowSelectionMenu(menu));
//
//
//            InvertGraphEditor.WindowManager.InitItemWindow(types.ToArray(),,command.AllowNone);
        }
        public virtual IEnumerable<GraphTypeInfo> GetRelatedTypes(SelectTypeCommand command)
        {
            if (command.AllowNone)
            {
                yield return new GraphTypeInfo() { Name = null, Group = "", Label = "[ None ]" };
            }
            if (command.IncludePrimitives)
            {
                var itemTypes = InvertGraphEditor.TypesContainer.ResolveAll<GraphTypeInfo>();
                foreach (var elementItemType in itemTypes)
                {
                    yield return elementItemType;
                }
            }
            foreach (var item in command.AdditionalTypes)
            {
                yield return item;
            }
  
            if (command.PrimitiveOnly) yield break;

            foreach (var item in Repository.AllOf<IClassTypeNode>())
            {
                if (item.Graph != null)
                yield return new GraphTypeInfo() { Name = item.Identifier, Group = item.Graph.Name, Label = item.Name };
            }
        }
    }
}
