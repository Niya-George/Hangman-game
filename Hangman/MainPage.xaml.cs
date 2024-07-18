
using System.ComponentModel;
using System.Diagnostics;

namespace Hangman
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {


        #region UI Properties
        public string Spotlight
        {
            get => spotlight;

            set { spotlight = value; OnPropertyChanged(); }
        }

        public List<char> Letters { get => letters; set { letters = value; OnPropertyChanged(); } }

        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public string GameStatus
        {
            get => gamestatus;
            set
            {
                gamestatus = value;
                OnPropertyChanged();

            }
        }

        public string CurrentImage
        {
            get => currentimage;
            set
            {
                currentimage = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Fields
        List<string> words = new List<string>()  //list of random words
            {
            "dog",
            "monkey",
            "sheep",
            "horse",
            "tiger",
            "lion",
            "giraffe",
            "bear",
            "goat",
            "elephant"
            };

        List<char> guessed = new List<char>();

        string answer = " ";  //for storing random word
        private string spotlight;
        private List<char> letters = new List<char>();
        private string message;
        int mistakes = 0;
        int maxWrong = 6;
        private string gamestatus;
        private string currentimage = "img0.jpg";
        #endregion

        public MainPage()
        {
            InitializeComponent();
            Letters.AddRange("abcdefghijklmnopqrstuvwxyz");
            BindingContext = this;
            PickWord();
            CalculateWord(answer, guessed);
        }

        private void PickWord()  
        {
          
            answer = words[new Random().Next(words.Count)];
            Debug.WriteLine(answer);

        }

        private void CalculateWord(string answer, List<char> guessed)
        {
            var temp = answer.Select(x => (guessed.IndexOf(x) >= 0 ? x : '_')).ToArray();

            Spotlight = string.Join(' ', temp);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                var letter = btn.Text;
                btn.IsEnabled = false;
                HandleGuess(letter[0]);
            }
        }

        private void HandleGuess(char letter)
        {
            if (guessed.IndexOf(letter) == -1)
            {
                guessed.Add(letter);
            }
            if (answer.IndexOf(letter) >= 0)
            {
                CalculateWord(answer, guessed);
                CheckIfGameWon();
            }
            else if (answer.IndexOf(letter) == -1)
            {
                mistakes++;
                UpdateStatus();
                CheckIfGameLost();
                CurrentImage = $"img{mistakes}.jpg";
            }
        }

        private void CheckIfGameWon()
        {
            if (Spotlight.Replace(" ", "") == answer)
            {
                Message = "YOU WIN !";
                DisableLetters();
            }
        }

        private void DisableLetters()
        {
            
            foreach ( var children in Container.Children)
                {
                var btn = children as Button;
                if(btn !=  null )
                {
                    btn.IsEnabled = false;
                }
            }
         }

        private void EnableLetters()
        {

            foreach (var children in Container.Children)
            {
                var btn = children as Button;
                if (btn != null)
                {
                    btn.IsEnabled = true;
                }
            }
        }

        private void UpdateStatus()
        {
            GameStatus = $"Errors : {mistakes} of {maxWrong}";
        }

        private void CheckIfGameLost()
        {
            if (mistakes == maxWrong)
            {
                Message = $"YOU L0ST! Answer : {answer}";
                DisableLetters();
                
            }
        }

        

        private void Reset_Button(object sender, EventArgs e)
        {
            mistakes=0 ;
            guessed = new List<char>();
            CurrentImage = "Img0.jpg";
            PickWord();
            CalculateWord(answer, guessed);
            Message = " ";
            UpdateStatus();
            EnableLetters();


        }

        
    }
}
