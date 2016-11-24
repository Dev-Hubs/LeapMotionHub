using Leap;
using LeapFirst;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LeapMotionHub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ILeapEventDelegate
    {
        private Controller controller = new Controller();
        private LeapEventListener listener;
        private Boolean isClosing = false;
        private Visualizer visualizer = new Visualizer();
        public MainWindow()
        {
            InitializeComponent();
            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            controller.AddListener(listener);
        }

        public void LeapEventNotification(string EventName)
        {
            if (this.CheckAccess())
            {
                switch (EventName)
                {
                    case "onInit":
                        Console.WriteLine("onInit");
                        break;
                    case "onConnect":
                        this.connectHandler();
                        break;
                    case "onFrame":
                        if (!this.isClosing)
                            this.newFrameHandler(this.controller.Frame());
                        break;
                    case "onExit":
                        Console.WriteLine("onExit");
                        break;
                    case "onDisconnect":
                        Console.WriteLine("onDisconnect");
                        break;
                    default:
                        Console.WriteLine("unchecked event {0}",EventName);
                        break;
                }
            }
            else
            {
                Dispatcher.Invoke(new LeapEventDelegate(LeapEventNotification), new object[] { EventName });
            }
        }
        void connectHandler()
        {
            this.controller.SetPolicy(Controller.PolicyFlag.POLICY_IMAGES);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
            this.controller.Config.SetFloat("Gesture.Swipe.MinLength", 100.0f);
        }
        void newFrameHandler(Leap.Frame frame)
        {

            while (LeftHandModelGroup.Children.Count > 0)
                LeftHandModelGroup.Children.RemoveAt(0);
            while (RightHandModelGroup.Children.Count > 0)
                RightHandModelGroup.Children.RemoveAt(0);


            List<GeometryModel3D> leftList = new List<GeometryModel3D>();
            List<GeometryModel3D> rightList = new List<GeometryModel3D>();
            if (!frame.IsValid) return;

            if (frame.Hands.Count == 2)
            {
                Hand hand = frame.Hands[0];
                Hand hand2 = frame.Hands[1];
                if (hand.IsLeft)
                {
                    leftList = visualizer.drawLeftHand(hand);
                    rightList = visualizer.drawRightHand(hand2);

                }
                else
                {
                    leftList = visualizer.drawLeftHand(hand2);
                    rightList = visualizer.drawRightHand(hand);
                }
            }
            else if (frame.Hands.Count == 1) {
                Hand hand = frame.Hands[0];
                if (hand.IsLeft)
                {
                    leftList = visualizer.drawLeftHand(hand);
                }
                else
                {
                    rightList = visualizer.drawRightHand(hand);
                }
            }
            else {
            }
            leftList.ForEach(delegate (GeometryModel3D m)
            {
                LeftHandModelGroup.Children.Add(m);

            });
            rightList.ForEach(delegate (GeometryModel3D m)
            {

                RightHandModelGroup.Children.Add(m);

            });
        }
    }
}
