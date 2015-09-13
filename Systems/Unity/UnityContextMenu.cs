using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Invert.Core.GraphDesigner.Unity
{
    public class UnityContextMenu : ContextMenuUI
    {
        public override void AddSeparator()
        {
            base.AddSeparator();
            Commands.Add(new ContextMenuItem() {Title = string.Empty});
        }

        public void CreateMenuItems(GenericMenu genericMenu)
        {
            var groups = Commands.GroupBy(p => p==null? "" : p.Group).OrderBy(p => p.Key == "Default").ToArray();

            foreach (var group in groups)
            {

                //genericMenu.AddDisabledItem(new GUIContent(group.Key));
                var groupCount = 0;
                foreach (var editorCommand in group.OrderBy(p => p.Order))
                {
                    ICommand command = editorCommand.Command;
                    genericMenu.AddItem(new GUIContent(editorCommand.Title),editorCommand.Checked, () =>
                    {
                        InvertApplication.Execute(command);
                    } );
                  
                }
                if (group != groups.Last() && groupCount > 0)
                    genericMenu.AddSeparator("");
            }
        }

        public override void Go()
        {
            base.Go();
            var genericMenu = new GenericMenu();
            CreateMenuItems(genericMenu);
            genericMenu.ShowAsContext();
        }
    }
}