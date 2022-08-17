using System;
using System.Collections.Generic;
using System.Threading;

namespace finalproject
{
    class Player
    {
        private string name;
        private readonly int playerId; //readonly類似const。不過readonly不須提供初始值，可在建構子中賦值
        private List<int> handCard = new List<int>(); //用整數形式存手牌，較好判斷數字與花色。印出來時再用dictionary轉成文字
        public List<int> coveredCard = new List<int>();
        private static int topCard;
        private static readonly Dictionary<int, string> int2poker = new Dictionary<int, string>() {
            { 0, "Ace of Spades" }, { 1, "2 of Spades" }, { 2, "3 of Spades" }, { 3, "4 of Spades" }, { 4, "5 of Spades" }, { 5, "6 of Spades" }, { 6, "7 of Spades" }, { 7, "8 of Spades" }, { 8, "9 of Spades" }, { 9, "10 of Spades" }, { 10, "Jack of Spades" }, {11, "Queen of Spades" }, { 12, "King of Spades" },
            { 13, "Ace of Hearts" }, { 14, "2 of Hearts" }, { 15, "3 of Hearts" }, { 16, "4 of Hearts" }, { 17, "5 of Hearts" }, { 18, "6 of Hearts" }, { 19, "7 of Hearts" }, { 20, "8 of Hearts" }, { 21, "9 of Hearts" }, { 22, "10 of Hearts" }, { 23, "Jack of Hearts" }, {24, "Queen of Hearts" }, { 25, "King of Hearts" },
            { 26, "Ace of Diamonds" }, { 27, "2 of Diamonds" }, { 28, "3 of Diamonds" }, { 29, "4 of Diamonds" }, { 30, "5 of Diamonds" }, { 31, "6 of Diamonds" }, { 32, "7 of Diamonds" }, { 33, "8 of Diamonds" }, { 34, "9 of Diamonds" }, { 35, "10 of Diamonds" }, { 36, "Jack of Diamonds" }, {37, "Queen of Diamonds" }, { 38, "King of Diamonds" },
            { 39, "Ace of Clubs" }, { 40, "2 of Clubs" }, { 41, "3 of Clubs" }, { 42, "4 of Clubs" }, { 43, "5 of Clubs" }, { 44, "6 of Clubs" }, { 45, "7 of Clubs" }, { 46, "8 of Clubs" }, { 47, "9 of Clubs" }, { 48, "10 of Clubs" }, { 49, "Jack of Clubs" }, {50, "Queen of Clubs" }, { 51, "King of Clubs" }};

        public Player(int[] cardDeck, ref int currentCard, string name, int id)
        {
            this.name = name;
            playerId = id;
            while (handCard.Count < 5) //一開始先抽五張手牌
                DrawACard(cardDeck, ref currentCard);
        }
        public string Name
        {
            get { return name; }
        }
        public int PlayerId
        {
            get { return playerId; }
        }
        public int HandCardNum
        {
            get { return handCard.Count; }
        }
        public static int TopCard
        {
            set { topCard = value; }
        }
        private void ComputerTurn(int[] cardDeck, ref int currentCard) //id = 0 時進入
        {
            Console.WriteLine("\n輪到電腦");
            ComputerThinking();
            /*  //取消此段註解可以看電腦的出牌情況
            Console.WriteLine("牌頂: {0}", int2poker[topCard]);
            PrintCards("電腦手牌", handCard);
            */
            if (CanIDealACard2() == -1)
            {
                bool turnOver;
                Random rnd = new Random();
                do
                {
                    turnOver = Check(ref topCard, rnd.Next() % handCard.Count + 1);
                } while (!turnOver);
                if (currentCard < cardDeck.Length && handCard.Count < 5)
                    DrawACard(cardDeck, ref currentCard);
                if (topCard % 13 == 10) //電腦出了Jack，要決定花色與數字，花色決定為電腦最多的花色，數字為亂數決定
                {
                    int specificCard = 0;
                    int[] fourSuitsCount = new int[] { 0, 0, 0, 0 };
                    for (int i = 0; i < handCard.Count; i++)
                        fourSuitsCount[handCard[i] / 13]++;
                    int[] temp = new int[fourSuitsCount.Length];
                    Array.Copy(fourSuitsCount, temp, fourSuitsCount.Length);
                    Array.Sort(temp);
                    specificCard += Array.IndexOf(fourSuitsCount, temp[^1]) * 13; //^1: 從後面數來第一個
                    specificCard += rnd.Next() % 13;
                    topCard = specificCard;
                    Console.WriteLine("電腦出了Jack!");
                }
            }
            else if (CanIDealACard2() >= 0) //有其他牌可出，不需出Jack(亂數選到Jack就跳過)
            {
                bool turnOver;
                Random rnd = new Random();
                do
                {
                    int whichHandCard = rnd.Next() % handCard.Count + 1;
                    if (handCard[whichHandCard - 1] % 13 == 10)
                    {
                        turnOver = false;
                        continue;
                    }
                    turnOver = Check(ref topCard, whichHandCard);
                } while (!turnOver);
                if (currentCard < cardDeck.Length && handCard.Count < 5)
                    DrawACard(cardDeck, ref currentCard);
            }
            else
            {
                int whichCard = 1;
                for (int i = 0; i < handCard.Count; i++)
                {
                    if ((handCard[i] % 13) < (handCard[whichCard - 1] % 13))
                        whichCard = i + 1;
                }
                coveredCard.Add(handCard[whichCard - 1]);
                handCard.Remove(handCard[whichCard - 1]);
                if (currentCard < cardDeck.Length && handCard.Count < 5)
                    DrawACard(cardDeck, ref currentCard);
            }
            /*  //取消此段註解可以看電腦的出牌情況
            PrintCards("電腦手牌", handCard);
            PrintCards("電腦蓋的牌", coveredCard);
            */
        }
        private void ComputerThinking() //純粹只是讓電腦看起來有在思考
        {
            Console.WriteLine();
            Thread.Sleep(700);
            for (int i = 0; i < 3; i++)
            {
                Console.Write(". ");
                Thread.Sleep(500);
            }
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
        public void MyTurn(int[] cardDeck, ref int currentCard)
        {
            if (playerId == 0)
            {
                ComputerTurn(cardDeck, ref currentCard);
                return;
            }
            Console.WriteLine("\n輪到{0}，牌頂為: {1}", name, int2poker[topCard]);
            PrintCards("我的手牌", handCard);
            if (CanIDealACard2() >= -1)
            {
                Console.WriteLine("出牌還是蓋牌: ");
                switch (SelectList(new string[] { "蓋牌", "出牌" }))
                {
                    case 0: Console.WriteLine("請選擇要蓋哪張牌: "); CoverACard(); break;
                    case 1: Console.WriteLine("請選擇要出哪張牌: "); DealACard(); break;
                    default: Console.WriteLine("錯誤訊息: 出牌蓋牌"); break;
                }
            }
            else
            {
                Console.WriteLine("請選擇要蓋哪張牌: ");
                CoverACard();
            }
            if (topCard % 13 == 10) //頂牌為Joker要指定花色和數字
            {
                int specificCard = 0;
                Console.WriteLine("請指定花色:");
                specificCard += 13 * SelectList(new string[] { "Spades", "Hearts", "Diamonds", "Clubs" });
                Console.WriteLine("輸入請指定數字(1~13):");
                specificCard += int.Parse(Console.ReadLine()) - 1;
                topCard = specificCard;
            }
            if (currentCard < cardDeck.Length && handCard.Count < 5) //如果牌堆還有牌且手牌<5張，則抽牌
                DrawACard(cardDeck, ref currentCard);
            PrintCards("已蓋的牌", coveredCard);
        }
        public void PrintCards(string title, List<int> handOrCovered)
        {
            Console.Write("{0}: ", title);
            for (int i = 0; i < handOrCovered.Count; i++)
            {
                if (i > 0)
                    Console.Write(", ");
                Console.Write("{0}", int2poker[handOrCovered[i]]);
            }
            Console.WriteLine();
        }
        private int CanIDealACard2() //不能出牌:回傳-2；能出牌且不用考慮J(沒有J或只有J):回傳-1；能出牌且要考慮J: 回傳0
        {
            bool canIDealACard = false;
            bool haveJack = false;
            int jackCount = 0;
            List<int> available = new List<int>();
            for (int i = 0; i < handCard.Count; i++)
            {
                if (((topCard % 13) == (handCard[i] % 13)) || ((topCard / 13) == (handCard[i] / 13)) || (handCard[i] % 13 == 10)) //檢查數字相同或是花色相同或是有Jack
                {
                    canIDealACard = true;
                    available.Add(handCard[i]);
                    if (handCard[i] % 13 == 10)
                    {
                        jackCount++;
                        haveJack = true;
                    }
                }
            }
            if (!canIDealACard)
                return -2;
            else if ((canIDealACard && !haveJack) || (canIDealACard && haveJack && available.Count == jackCount))
                return -1;
            else
                return 0;
        }
        private void DrawACard(int[] cardDeck, ref int currentCard) //一次只抽一張卡
        {
            handCard.Add(cardDeck[currentCard]);
            currentCard++;
        }
        private string[] HandCard2String()
        {
            string[] handCardNames = new string[handCard.Count];
            for (int i = 0; i < handCardNames.Length; i++)
                handCardNames[i] = int2poker[handCard[i]];
            return handCardNames;
        }
        private bool Check(ref int topCard, int dealtCard)
        {
            if (((topCard % 13) == (handCard[dealtCard - 1] % 13)) || ((topCard / 13) == (handCard[dealtCard - 1] / 13)) || (handCard[dealtCard - 1] % 13 == 10)) //檢查數字相同或是花色相同或是Jack
            {
                topCard = handCard[dealtCard - 1];
                handCard.Remove(handCard[dealtCard - 1]);
                return true;
            }
            else
            {
                if (playerId != 0)
                    Console.WriteLine("此牌不能出!");
                return false;
            }
        }
        private void DealACard() //出牌
        {
            bool turnOver;
            do
            {
                turnOver = Check(ref topCard, SelectList(HandCard2String()) + 1);
            } while (!turnOver);
        }
        private void CoverACard() //蓋牌
        {
            int whichCard = SelectList(HandCard2String()) + 1;
            coveredCard.Add(handCard[whichCard - 1]);
            handCard.Remove(handCard[whichCard - 1]);
        }
        public static int SelectList(string[] nameList) //選單式
        {
            bool successSelect = false;
            int nameSelected = 0;
            RefreshList(nameList, nameSelected);
            while (!successSelect)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        nameSelected = (nameSelected - 1 < 0) ? nameList.Length - 1 : nameSelected - 1;
                        Clear(nameList.Length);
                        RefreshList(nameList, nameSelected);
                        break;
                    case ConsoleKey.DownArrow:
                        nameSelected = (nameSelected + 1 >= nameList.Length) ? 0 : nameSelected + 1;
                        Clear(nameList.Length);
                        RefreshList(nameList, nameSelected);
                        break;
                    case ConsoleKey.Enter:
                        successSelect = true;
                        break;
                    default:
                        break;
                }
            }
            return nameSelected;
        }
        private static void RefreshList(string[] nameList, int nameSelected)
        {
            for (int i = 0; i < nameList.Length; i++)
            {
                if (i == nameSelected)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(nameList[i]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(nameList[i]);
                }
            }
        }
        public static void Clear(int num)
        {
            for (int i = 0; i < num; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop);
            }
        }
    }

}
