using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using WpfKit.ViewModelKit;

namespace CSTPad.Model
{
    public class IntellisencePopupBehavior : Behavior<Popup>
    {
        public ICommand FocusCommand => new ActionCommand(_ =>
        {
            var grid = AssociatedObject.Child as Grid;
            var listBox = grid.Children[0] as ListBox;

            AssociatedObject.PlacementTarget = TextBox;
            AssociatedObject.PlacementRectangle =
                TextBox.GetRectFromCharacterIndex(TextBox.CaretIndex);

            listBox.Focus();
        });
        
        public TextBox TextBox
        {
            get { return (TextBox)GetValue(TextBoxProperty); }
            set { SetValue(TextBoxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextBox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBoxProperty =
            DependencyProperty.Register("TextBox", typeof(TextBox), typeof(IntellisencePopupBehavior), new PropertyMetadata(null));
        
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Opened += (sender, e) =>
            {
                AssociatedObject.PlacementTarget = TextBox;
                AssociatedObject.PlacementRectangle =
                    TextBox.GetRectFromCharacterIndex(TextBox.CaretIndex);
            };

            AssociatedObject.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    AssociatedObject.IsOpen = false;
                    TextBox.Focus();
                }
                else if (e.Key == Key.Up || e.Key == Key.Down)
                {

                }
                else
                {
                    AssociatedObject.IsOpen = false;
                    TextBox.Focus();
                }
            };
        }
    }
}
