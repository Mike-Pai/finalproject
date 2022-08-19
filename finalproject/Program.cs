using System;
using System.Collections.Generic;
using System.Threading;

namespace finalproject
{
    class Program
    {
        static void Main(string[] args)
        {
            const int gameQuantity = 4;
            string[] gameList = new string[] { "game1-推倒提基", "game2-變色龍", "game3-圈圈叉叉", "game4-四子棋" };
            int gameSelect = 0;
            int win = 0, lose = 0, tie = 0;
            bool quit = false;

            RefreshList(gameList, gameSelect, gameQuantity, win, lose, tie);
            while (!quit)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        gameSelect--;
                        if (gameSelect < 0) gameSelect = gameQuantity - 1;
                        RefreshList(gameList, gameSelect, gameQuantity, win, lose, tie);
                        break;

                    case ConsoleKey.DownArrow:
                        gameSelect++;
                        if (gameSelect > gameQuantity - 1) gameSelect = 0;
                        RefreshList(gameList, gameSelect, gameQuantity, win, lose, tie);
                        break;

                    case ConsoleKey.Enter:
                        switch (gameSelect)
                        {
                            case 0:
                                Console.Clear();
                                TikiTopple(ref win, ref lose, ref tie);
                                Console.WriteLine("按下enter鍵再玩一次;按方向鍵選擇其他遊戲;按下esc鍵結束程序");
                                break;
                            case 1:
                                Console.Clear();
                                Chameleon(ref win, ref lose, ref tie);
                                Console.WriteLine("按下enter鍵再玩一次;按方向鍵選擇其他遊戲;按下esc鍵結束程序");
                                break;
                            case 2:
                                Console.Clear();
                                Tic_Tac_Toe(ref win, ref lose, ref tie);
                                Console.WriteLine("按下enter鍵再玩一次;按方向鍵選擇其他遊戲;按下esc鍵結束程序");
                                break;
                            case 3:
                                Console.Clear();
                                Connect_Four(ref win, ref lose, ref tie);
                                Console.WriteLine("按下enter鍵再玩一次;按方向鍵選擇其他遊戲;按下esc鍵結束程序");
                                break;
                            default:
                                break;
                        }
                        break;

                    case ConsoleKey.Escape:
                        quit = true;
                        break;

                    default:
                        break;
                }
            }
            Console.WriteLine("啊遊戲結束，下次見");
        }

        static void RefreshList(string[] gameList, int gameSelect, int gameQuantity, int win, int lose, int tie)
        {
            Console.Clear();
            Console.WriteLine("目前戰績{0}勝{1}敗{2}平手", win, lose, tie);
            Console.WriteLine("請選擇要玩的遊戲");
            int i;
            for (i = 0; i < gameQuantity; i++)
            {
                if (i == gameSelect)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(gameList[i]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else Console.WriteLine(gameList[i]);
            }
        }

        //**********************************************************推倒堤基**********************************************************************************
        static void TikiTopple(ref int win, ref int lose, ref int tie)
        {
            List<string> Tiki = new List<string>() { "紅色", "咖啡色", "黃色", "綠色", "淺藍色", "深藍色", "紫色" };
            int tikiLength = Tiki.Count;

            List<string> playerHand = new List<string>() { "上移三層", "上移一層", "上移一層", "移到最下層", "移除最底部的方塊" };
            List<string> comHand = new List<string>() { "上移三層", "上移一層", "上移一層", "移到最下層", "移除最底部的方塊" };
            int cardQuantity = 5;

            int playerScore = 0, comScore = 0;

            List<string> playerGoal = new List<string>(); //設定目標堤基
            List<string> comGoal = new List<string>();
            Random rand = new Random();
            PickPlayerGoal(rand, Tiki, tikiLength, ref playerGoal);
            PickComGoal(rand, Tiki, tikiLength, ref comGoal);

            while (cardQuantity != 0)
            {
                ComPlay(rand, comHand, comGoal, playerGoal, ref Tiki);
                PlayerPlay(playerGoal, ref playerHand, ref cardQuantity, ref Tiki);
            }

            Console.WriteLine("電腦的目標是{0}、{1}、{2}\n", comGoal[0], comGoal[1], comGoal[2]);
            CountFinalScore(Tiki, playerGoal, out playerScore);
            CountFinalScore(Tiki, comGoal, out comScore);
            if (playerScore > comScore)
            {
                Console.WriteLine("玩家{0}分、電腦{1}分，你贏了!", playerScore, comScore);
                win++;
            }
            else if (playerScore < comScore)
            {
                Console.WriteLine("玩家{0}分、電腦{1}分，你輸了...", playerScore, comScore);
                lose++;
            }
            else
            {
                Console.WriteLine("玩家{0}分、電腦{1}分，平手", playerScore, comScore);
                tie++;
            }
        }

        //設定目標堤基
        static void PickPlayerGoal(Random rand, List<string> Tiki, int tikiLength, ref List<string> playerGoal)
        {
            int First, Second, Third;

            First = rand.Next(0, tikiLength);
            if (First + 2 >= tikiLength) Second = (First + 2) - tikiLength;
            else Second = First + 2;
            if (First + 4 >= tikiLength) Third = (First + 4) - tikiLength;
            else Third = First + 4;

            playerGoal.Add(Tiki[First]);
            playerGoal.Add(Tiki[Second]);
            playerGoal.Add(Tiki[Third]);
        }
        static void PickComGoal(Random rand, List<string> Tiki, int tikiLength, ref List<string> comGoal)
        {
            int First, Second, Third;

            First = rand.Next(0, tikiLength);
            if (First + 1 >= tikiLength) Second = (First + 1) - tikiLength;
            else Second = First + 1;
            if (First + 3 >= tikiLength) Third = (First + 3) - tikiLength;
            else Third = First + 3;

            comGoal.Add(Tiki[First]);
            comGoal.Add(Tiki[Second]);
            comGoal.Add(Tiki[Third]);
        }



        //電腦出牌
        static void ComPlay(Random rand, List<string> comHand, List<string> comGoal, List<string> playerGoal, ref List<string> Tiki)
        {
            string movedTiki;
            switch (comHand[rand.Next(0, comHand.Count)])
            {
                case "上移三層":
                    ComUpThree(rand, comGoal, ref Tiki, ref comHand, out movedTiki);
                    Console.WriteLine("電腦對{0}方塊執行了\"上移三層\"", movedTiki);
                    break;

                case "上移一層":
                    ComUpOne(rand, comGoal, ref Tiki, ref comHand, out movedTiki);
                    Console.WriteLine("電腦對{0}方塊執行了\"上移一層\"", movedTiki);
                    break;

                case "移到最下層":
                    ComToBottom(rand, playerGoal, ref Tiki, ref comHand, out movedTiki);
                    Console.WriteLine("電腦對{0}方塊執行了\"移到最下層\"", movedTiki);
                    break;

                case "移除最底部的方塊":
                    ComRemoveBottom(ref Tiki, ref comHand);
                    Console.WriteLine("電腦移除了最底部的方塊");
                    break;

                default:
                    break;
            }
        }
        static void ComUpThree(Random rand, List<string> comGoal, ref List<string> Tiki, ref List<string> comHand, out string movedTiki)
        {
            int chosenGoal;
            string chosenTiki;

            do
            {
                chosenGoal = rand.Next(0, 3);//從目標堤基中選擇
                chosenTiki = comGoal[chosenGoal];//取得目標堤基名字
            } while (!Tiki.Contains(chosenTiki));//若名字不在遊戲板中就重選

            int tikiIndex = Tiki.IndexOf(chosenTiki);//取得目標堤基在遊戲板的索引
            Tiki.RemoveAt(tikiIndex);//從遊戲板上移除所選堤基
            if (tikiIndex - 3 < 0) Tiki.Insert(0, chosenTiki);//插入所選堤基在指定位置
            else Tiki.Insert(tikiIndex - 3, chosenTiki);

            comHand.RemoveAt(comHand.IndexOf("上移三層"));
            movedTiki = chosenTiki;
        }
        static void ComUpOne(Random rand, List<string> comGoal, ref List<string> Tiki, ref List<string> comHand, out string movedTiki)
        {
            int chosenGoal;
            string chosenTiki;

            do
            {
                chosenGoal = rand.Next(0, 3);//從目標堤基中選擇
                chosenTiki = comGoal[chosenGoal];//取得目標堤基名字
            } while (!Tiki.Contains(chosenTiki));//若名字不在遊戲板中就重選

            int tikiIndex = Tiki.IndexOf(chosenTiki);//取得目標堤基在遊戲板的索引
            Tiki.RemoveAt(tikiIndex);//從遊戲板上移除所選堤基
            if (tikiIndex - 1 < 0) Tiki.Insert(0, chosenTiki);//插入所選堤基在指定位置
            else Tiki.Insert(tikiIndex - 1, chosenTiki);

            comHand.RemoveAt(comHand.IndexOf("上移一層"));
            movedTiki = chosenTiki;
        }
        static void ComToBottom(Random rand, List<string> playerGoal, ref List<string> Tiki, ref List<string> comHand, out string movedTiki)
        {
            int chosenGoal;
            string chosenTiki;

            do
            {
                chosenGoal = rand.Next(0, 3);//從目標堤基中選擇
                chosenTiki = playerGoal[chosenGoal];//取得目標堤基名字
            } while (!Tiki.Contains(chosenTiki));//若名字不在遊戲板中就重選

            int tikiIndex = Tiki.IndexOf(chosenTiki);//取得目標堤基在遊戲板的索引
            Tiki.RemoveAt(tikiIndex);//從遊戲板上移除所選堤基
            Tiki.Add(chosenTiki);//插入所選堤基到底部

            comHand.RemoveAt(comHand.IndexOf("移到最下層"));
            movedTiki = chosenTiki;
        }
        static void ComRemoveBottom(ref List<string> Tiki, ref List<string> comHand)
        {
            Tiki.RemoveAt(Tiki.Count - 1);
            comHand.RemoveAt(comHand.IndexOf("移除最底部的方塊"));
        }

        //玩家出牌
        static void PlayerPlay(List<string> playerGoal, ref List<string> playerHand, ref int cardQuantity, ref List<string> Tiki)
        {
            int cardSelect = 0;
            bool roundOver = false;

            RefreshScreen(Tiki, playerGoal, cardQuantity, cardSelect, playerHand);
            while (!roundOver)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.RightArrow:
                        Console.Clear();
                        cardSelect++;
                        if (cardSelect >= cardQuantity) cardSelect = 0;
                        RefreshScreen(Tiki, playerGoal, cardQuantity, cardSelect, playerHand);
                        break;
                    case ConsoleKey.LeftArrow:
                        Console.Clear();
                        cardSelect--;
                        if (cardSelect < 0) cardSelect = cardQuantity - 1;
                        RefreshScreen(Tiki, playerGoal, cardQuantity, cardSelect, playerHand);
                        break;
                    case ConsoleKey.Enter:
                        switch (playerHand[cardSelect])
                        {
                            //, "上移一層", "上移一層", "移到最下層", "移除最底部的方塊"
                            case "上移三層":
                                PlayerUpThree(ref Tiki);
                                playerHand.Remove("上移三層");
                                cardQuantity--;
                                Console.Clear();

                                break;
                            case "上移一層":
                                PlayerUpOne(ref Tiki);
                                playerHand.Remove("上移一層");
                                cardQuantity--;
                                Console.Clear();
                                break;
                            case "移到最下層":
                                PlayerToBottom(ref Tiki);
                                playerHand.Remove("移到最下層");
                                cardQuantity--;
                                Console.Clear();
                                break;
                            case "移除最底部的方塊":
                                Tiki.RemoveAt(Tiki.Count - 1);
                                playerHand.Remove("移除最底部的方塊");
                                cardQuantity--;
                                Console.Clear();
                                break;
                        }
                        roundOver = true;
                        break;
                }
            }
            if (cardQuantity != 0) Console.WriteLine("輪到電腦");
            else
            {
                Console.WriteLine("遊戲結束!");
                RefreshScreen(Tiki, playerGoal, cardQuantity, cardSelect, playerHand);
            }
        }
        static void RefreshScreen(List<string> Tiki, List<string> playerGoal, int cardQuantity, int cardSelect, List<string> playerHand)
        {
            Console.Write("\n");
            foreach (string tiki in Tiki)
            {
                switch (tiki)
                {
                    case "紅色":
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("■");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case "咖啡色":
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("■");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case "黃色":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("■");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case "綠色":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("■");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case "淺藍色":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("■");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case "深藍色":
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine("■");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case "紫色":
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine("■");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
            Console.WriteLine("\n{0}在第一格得5分、{1}在第二格以上得4分、{2}在第三格以上得3分", playerGoal[0], playerGoal[1], playerGoal[2]);
            if (cardQuantity != 0) Console.WriteLine("\n請選擇你要出的牌:\n");
            int i;
            for (i = 0; i < cardQuantity; i++)
            {
                if (i == cardSelect)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("{0}  ", playerHand[i]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else Console.Write("{0}  ", playerHand[i]);
            }
            Console.Write("\n");
        }
        static void PlayerUpThree(ref List<string> Tiki)
        {
            string target;
            int targetIndex;

            Console.WriteLine("請輸入使用對象的方塊顏色");
            do
            {
                target = Console.ReadLine();
                if (Tiki.Contains(target)) break;
                else Console.WriteLine("請輸入有效目標名稱");
            } while (true);

            targetIndex = Tiki.IndexOf(target);
            Tiki.RemoveAt(targetIndex);
            if (targetIndex - 3 < 0) Tiki.Insert(0, target);
            else Tiki.Insert(targetIndex - 3, target);
        }
        static void PlayerUpOne(ref List<string> Tiki)
        {
            string target;
            int targetIndex;

            Console.WriteLine("請輸入使用對象的方塊顏色");
            do
            {
                target = Console.ReadLine();
                if (Tiki.Contains(target)) break;
                else Console.WriteLine("請輸入有效目標名稱");
            } while (true);

            targetIndex = Tiki.IndexOf(target);
            Tiki.RemoveAt(targetIndex);
            if (targetIndex - 1 < 0) Tiki.Insert(0, target);
            else Tiki.Insert(targetIndex - 1, target);
        }
        static void PlayerToBottom(ref List<string> Tiki)
        {
            string target;

            Console.WriteLine("請輸入使用對象的方塊顏色");
            do
            {
                target = Console.ReadLine();
                if (Tiki.Contains(target)) break;
                else Console.WriteLine("請輸入有效目標名稱");
            } while (true);

            Tiki.Remove(target);
            Tiki.Add(target);
        }

        //算分數
        static int CountFinalScore(List<string> Tiki, List<string> Goal, out int score)
        {
            int sum = 0;
            if (Goal[0] == Tiki[0]) sum += 5;
            if (Goal[1] == Tiki[0] || Goal[1] == Tiki[1]) sum += 4;
            if (Goal[2] == Tiki[0] || Goal[2] == Tiki[1] || Goal[2] == Tiki[2]) sum += 3;
            score = sum;
            return score;
        }

        //**************************************************************變色龍******************************************************************************
        static int[] Shuffle(int cardNum)
        {
            int[] cardDeck = new int[cardNum];
            Random rnd = new Random();
            for (int i = 0; i < cardNum; i++)
            {
                do
                {
                    cardDeck[i] = rnd.Next() % cardNum;
                } while (Array.IndexOf(cardDeck, cardDeck[i]) != i);
            }
            return cardDeck;
        }
        static void Chameleon(ref int win, ref int lose, ref int tie)
        {
            int cardNum = 52; //牌的數量
            int[] cardDeck = Shuffle(cardNum); //洗好的牌 (固定)
            int currentCard = 0; //現在要抽第幾張卡 (浮動)
            Player.TopCard = cardDeck[currentCard];
            currentCard++;
            Player computer = new Player(cardDeck, ref currentCard, "computer", 0);
            Player user = new Player(cardDeck, ref currentCard, "user", 1);
            Player[] player_array = new Player[2];
            player_array[0] = computer;
            player_array[1] = user;
            Random rnd = new Random();
            int currentPlayer = rnd.Next() % player_array.Length; //亂數決定第一個玩家
            int howManyPlayersAlive = player_array.Length;
            Console.WriteLine("規則說明:\n1.出牌者必須打出與上一張打出的牌有同樣花色或點數的牌或是變色龍牌。" +
                "\n2.四種花色的J為變色龍牌，出牌者可以無視上一張打出的牌的花色或點數，並且選擇這張牌代表的花色和點數。" +
                "\n3.若出牌者無牌可出或是不想出牌，則從手牌中選一張背面朝上蓋在自己面前。" +
                "\n4.無論出牌還是蓋牌，都必須立即摸牌，將自己的手牌補滿5張(若牌堆無牌可摸，則不摸牌)。" +
                "\n5.當全部玩家皆無手牌時，本局遊戲結束，開始計分。" +
                "\n6.所有玩家將自己的蓋牌亮出，牌面點數相加即為自己該局的積分，積分較低者獲勝。" +
                "\n\n按Enter鍵以繼續...");
            Console.ReadLine();
            Player.Clear(2);
            Console.WriteLine("\n變色龍遊戲開始!!!");

            while (howManyPlayersAlive != 0)
            {
                player_array[currentPlayer].MyTurn(cardDeck, ref currentCard);
                if (player_array[currentPlayer].HandCardNum == 0)
                {
                    howManyPlayersAlive--;
                }
                currentPlayer++;
                if (currentPlayer >= player_array.Length)
                    currentPlayer = 0;
            }

            Console.WriteLine();
            int[] scores = new int[player_array.Length]; //計分
            int min = 100000;
            for (int i = 0; i < player_array.Length; i++)
            {
                int score = 0;
                for (int j = 0; j < player_array[i].coveredCard.Count; j++)
                    score += player_array[i].coveredCard[j] % 13 + 1;
                scores[i] = score;
                if (scores[i] < min)
                    min = scores[i];
                Console.WriteLine("玩家 {0} 積分: {1}", player_array[i].Name, scores[i]);
                player_array[i].PrintCards("已蓋的牌", player_array[i].coveredCard);
                Console.WriteLine();
            }

            Player winner = player_array[Array.IndexOf(scores, min)];
            if (scores[0] == scores[1])
                tie++;
            else if (winner.PlayerId == 0)
                lose++;
            else
                win++;
            Console.WriteLine("\n恭喜 {0} 獲勝!!!", winner.Name);
        }


        //*************************************************************圈圈叉叉*******************************************************************************
        static void Print(char[,] board)
        {
            Console.WriteLine("\n   0   1   2");
            Console.WriteLine("            ");
            Console.WriteLine("0  {0} | {1} | {2}", board[0, 0], board[0, 1], board[0, 2]);
            Console.WriteLine("  ---+---+---");
            Console.WriteLine("1  {0} | {1} | {2}", board[1, 0], board[1, 1], board[1, 2]);
            Console.WriteLine("  ---+---+---");
            Console.WriteLine("2  {0} | {1} | {2}\n", board[2, 0], board[2, 1], board[2, 2]);
        }
        static void UserTurn(ref char[,] board, ref int[] oCount, ref int totalTurn) //user: o
        {
            Console.Write("換你下(請先輸入左方座標，再輸入上方做標，並以逗號隔開): ");
            int i = 0;
            int j = 0;
            bool successPut = false;
            do
            {
                do
                {
                    string[] userInput;
                    try
                    {
                        userInput = Console.ReadLine().Split(',');
                        i = int.Parse(userInput[0]);
                        j = int.Parse(userInput[1]);
                        successPut = true;
                    }
                    catch
                    {
                        Console.WriteLine("輸入格式錯誤，請重新輸入!");
                        successPut = false;
                        continue;
                    }
                } while (!successPut);
                successPut = false;
                if ((i > 2 || i < -1) || (j > 2 || j < 0))
                {
                    Console.WriteLine("無此格，請下另一格!");
                    continue;
                }
                else if (board[i, j] != ' ')
                    Console.WriteLine("此格已被填滿，請下另一格!");
                else
                    successPut = true;
            } while (!successPut);
            Put(ref board, ref oCount, i, j, 'o', ref totalTurn);
        }
        static void ComputerTurn(ref char[,] board, ref int[] xCount, ref int totalTurn, int[] oCount) //computer: x
        {
            Console.WriteLine("換電腦下:");
            ComputerThinking();
            ComputerThinking();
            int totalTurnNow = totalTurn;
            //電腦判斷1: 電腦差一顆就贏
            for (int i = 0; i < 8; i++)
            {
                bool isVertical = false;
                if (xCount[i] == 2 && oCount[i] == 0)
                {
                    AccessSituation(i, ref isVertical, ref board, ref xCount, ref totalTurn);
                    if (isVertical)
                        i += 3;
                }
                if (totalTurn == totalTurnNow + 1) //擋一顆就好
                    break;
            }
            //電腦判斷2: 玩家差一顆就贏時，電腦要擋
            for (int i = 0; i < 8; i++)
            {
                bool isVertical = false;
                if (oCount[i] == 2 && xCount[i] == 0)
                {
                    AccessSituation(i, ref isVertical, ref board, ref xCount, ref totalTurn);
                    if (isVertical)
                        i += 3;
                }
                if (totalTurn == totalTurnNow + 1) //擋一顆就好
                    break;
            }
            //電腦判斷2: 如果中間是空的，就下中間
            if (totalTurn == totalTurnNow && board[1, 1] == ' ')
                Put(ref board, ref xCount, 1, 1, 'x', ref totalTurn);
            //電腦判斷3: 隨機下
            if (totalTurn == totalTurnNow)
            {
                Random rnd = new Random();
                do
                {
                    int key = rnd.Next() % 9;
                    int i = key / 3;
                    int j = key % 3;
                    if (board[i, j] == ' ')
                        Put(ref board, ref xCount, i, j, 'x', ref totalTurn);
                } while (totalTurn == totalTurnNow);
            }
        }
        static void Put(ref char[,] board, ref int[] count, int i, int j, char xo, ref int totalTurn) //下一顆棋
        {
            board[i, j] = xo;
            count[i]++;
            count[j + 3]++;
            if (i == j)
                count[6]++;
            if (i + j == 2)
                count[7]++;
            totalTurn++;
            Print(board);
        }
        static void AccessSituation(int i, ref bool isVertical, ref char[,] board, ref int[] xCount, ref int totalTurn) //專門給ComputerTurn用來判斷哪一行差一個就要贏了，並且下在正確位置
        {
            if (i >= 3 && i <= 5) //直的
            {
                i -= 3;
                isVertical = true;
            }
            for (int j = 0; j < 3; j++)
            {
                switch (i)
                {
                    case 0:
                    case 1:
                    case 2:
                        if (isVertical == false && board[i, j] == ' ') //橫的判斷
                        {
                            Put(ref board, ref xCount, i, j, 'x', ref totalTurn);
                            break;
                        }
                        if (isVertical == true && board[j, i] == ' ') //直的判斷
                        {
                            Put(ref board, ref xCount, j, i, 'x', ref totalTurn);
                            break;
                        }
                        break;
                    case 6: //斜的判斷: \
                        if (board[j, j] == ' ')
                        {
                            Put(ref board, ref xCount, j, j, 'x', ref totalTurn);
                            break;
                        }
                        break;
                    case 7: //斜的判斷: /
                        if (board[j, 2 - j] == ' ')
                        {
                            Put(ref board, ref xCount, j, 2 - j, 'x', ref totalTurn);
                            break;
                        }
                        break;
                    default:
                        Console.WriteLine("錯誤訊息");
                        break;
                }
            }
        }
        static void ComputerThinking() //純粹只是讓電腦看起來有在思考
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
        static void Tic_Tac_Toe(ref int win, ref int lose, ref int tie) //主程式在這!!!
        {
            char[,] board = {
                { ' ', ' ', ' ' },
                { ' ', ' ', ' ' },
                { ' ', ' ', ' ' }};
            int totalTurn = 0;
            bool userWin = false;
            bool computerWin = false;
            int[] oCount = { 0, 0, 0, 0, 0, 0, 0, 0 }; //row0,row1,row2,col1,col2,col3,\,/
            int[] xCount = { 0, 0, 0, 0, 0, 0, 0, 0 };
            Print(board);

            while (!(userWin || computerWin || (totalTurn >= 9))) //結束遊戲條件是其中一方贏或9格下滿
            {
                UserTurn(ref board, ref oCount, ref totalTurn);
                if (Array.IndexOf(oCount, 3) >= 0)
                    userWin = true;
                if (userWin || (totalTurn >= 9)) break;
                ComputerTurn(ref board, ref xCount, ref totalTurn, oCount);
                if (Array.IndexOf(xCount, 3) >= 0)
                    computerWin = true;
            }

            if (userWin)
            {
                Console.WriteLine("\n你贏了");
                win++;
            }
            else if (computerWin)
            {
                Console.WriteLine("\n你輸了");
                lose++;
            }
            else
            {
                Console.WriteLine("\n平手");
                tie++;
            }
        }

        //*************************************************************四子棋*******************************************************************************
        static void Connect_Four(ref int win, ref int lose, ref int tie)
        {

            Console.WriteLine("歡迎來到四子棋!");
            Console.WriteLine("遊戲規則：");
            Console.WriteLine("1.棋盤大小為7x6。");
            Console.WriteLine("2.雙方必須輪流把一枚己棋選擇投入任一行，讓棋子落下在底部或其他棋子上。");
            Console.WriteLine("3.玩家輸入1~7代表選擇將棋放入哪一行。");
            Console.WriteLine("4.當己方4枚棋子以縱、橫、斜方向連成一線時獲勝。");
            Console.WriteLine("5.棋盤滿棋時，無任何連成4子，則平手。");
            Console.WriteLine("遊戲開始!");
            //定義玩家
            int nowplayer = 1;
            string[] playername = new string[] { "主玩家", "踢館者" };
            //定義棋盤及棋子
            char[,] checkerboard = new char[6, 7];
            char[] player_piece = { 'o', 'x' };
            char put_piece = player_piece[0];
            //定義變數
            bool check_full = false;
            bool is_board_all_full = false;
            bool is_win = false;
            int winner = 0;
            int player_put_position;
            //展示棋盤
            Console.WriteLine("  1   2   3   4   5   6   7  ");
            for (int i = 0; i < checkerboard.GetUpperBound(0) + 1; i++)
            {
                Console.WriteLine("+---+---+---+---+---+---+---+");
                Console.Write("|");
                for (int j = 0; j < checkerboard.GetUpperBound(1) + 1; j++)
                {
                    checkerboard[i, j] = ' ';
                    Console.Write(" {0} |", checkerboard[i, j]);
                }
                Console.WriteLine("");
            }
            Console.WriteLine("+---+---+---+---+---+---+---+");
            Console.WriteLine("  1   2   3   4   5   6   7  ");

            do
            {
                //確認棋盤空位
                is_board_all_full = check_all_full(checkerboard);
                if (is_board_all_full)
                {
                    break;
                }
                //玩家放入旗子
                Console.WriteLine("目前玩家:{0}", playername[nowplayer - 1]);
                Console.WriteLine("輸入放入第幾行(1~7)");
                try
                {
                    player_put_position = int.Parse(Console.ReadLine()) - 1;
                }
                catch
                {
                    Console.WriteLine("輸入錯誤！請重新輸入1~7行：");
                    continue;
                }
                //判斷輸入格式是否為數字
                if (!check_input(player_put_position))
                {
                    Console.WriteLine("輸入錯誤！請重新輸入1~7行：");
                    continue;
                }
                //判斷放入的欄位有沒有滿            
                check_full = check_board(checkerboard, player_put_position);
                if (!check_full)
                {
                    piece_down(ref checkerboard, put_piece, player_put_position);
                    show_board(checkerboard);
                }
                else
                {
                    Console.WriteLine("這一行滿了!請換一行!");
                    continue;
                }
                //確認勝利與否
                is_win = check_win(checkerboard);
                if (is_win)
                {
                    winner = nowplayer;
                }
                //改變玩家
                if (nowplayer == 1)
                {
                    nowplayer = 2;
                    put_piece = player_piece[1];
                }
                else
                {
                    nowplayer = 1;
                    put_piece = player_piece[0];
                }
            } while (!is_win);
            if (is_win)
            {
                Console.WriteLine("{0}獲勝!", playername[winner - 1]);
                if (winner == 1) win++;
                else if (winner == 2) lose++;
            }
            else if (is_board_all_full)
            {
                Console.WriteLine("此局平手!");
                tie++;
            }
        }

        private static bool check_all_full(char[,] checkerboard) //確認全部棋盤空格
        {
            bool isRowFull = true;
            int Row = 0;
            for (int j = 0; j < checkerboard.GetUpperBound(1) + 1; j++)
            {
                for (int i = 0; i < checkerboard.GetUpperBound(0) + 1; i++)
                {
                    if (checkerboard[i, j] != ' ')
                    {
                        Row = Row + 1;
                    }
                }
                if (Row != 6)
                {
                    isRowFull = false;
                    return isRowFull;
                }
                Row = 0;
            }
            return isRowFull;
        }

        private static bool check_win(char[,] checkerboard) //確認有無四個連續棋子(獲勝之條件)
        {
            bool win = false;
            for (int i = 0; i < checkerboard.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < checkerboard.GetUpperBound(1) + 1; j++)
                {
                    int check_win_case;
                    if (7 - j >= 4 & 6 - i >= 4)
                    {
                        check_win_case = 1;
                    }
                    else if (7 - j >= 4 & 6 - i < 4)
                    {
                        check_win_case = 2;
                    }
                    else if (7 - j < 4 & 6 - i >= 4)
                    {
                        check_win_case = 3;
                    }
                    else
                    {
                        check_win_case = 4;
                    }

                    switch (check_win_case)
                    {
                        case 1:
                            if (checkerboard[i, j] == checkerboard[i + 1, j] & checkerboard[i, j] == checkerboard[i + 2, j] & checkerboard[i, j] == checkerboard[i + 3, j] & checkerboard[i, j] != ' ')
                            {
                                win = true;
                                return win;
                            }
                            else if (checkerboard[i, j] == checkerboard[i, j + 1] & checkerboard[i, j] == checkerboard[i, j + 2] & checkerboard[i, j] == checkerboard[i, j + 3] & checkerboard[i, j] != ' ')
                            {
                                win = true;
                                return win;
                            }
                            else if (checkerboard[i, j] == checkerboard[i + 1, j + 1] & checkerboard[i, j] == checkerboard[i + 2, j + 2] & checkerboard[i, j] == checkerboard[i + 3, j + 3] & checkerboard[i, j] != ' ')
                            {
                                win = true;
                                return win;
                            }
                            else
                            {
                                win = false;
                                break;
                            }

                        case 2:
                            if (checkerboard[i, j] == checkerboard[i, j + 1] & checkerboard[i, j] == checkerboard[i, j + 2] & checkerboard[i, j] == checkerboard[i, j + 3] & checkerboard[i, j] != ' ')
                            {
                                win = true;
                                return win;
                            }
                            else if (checkerboard[i, j] == checkerboard[i - 1, j + 1] & checkerboard[i, j] == checkerboard[i - 2, j + 2] & checkerboard[i, j] == checkerboard[i - 3, j + 3] & checkerboard[i, j] != ' ')
                            {
                                win = true;
                                return win;
                            }
                            else
                            {
                                win = false;
                                break;
                            }

                        case 3:
                            if (checkerboard[i, j] == checkerboard[i + 1, j] & checkerboard[i, j] == checkerboard[i + 2, j] & checkerboard[i, j] == checkerboard[i + 3, j] & checkerboard[i, j] != ' ')
                            {
                                win = true;
                                return win;
                            }
                            else if (checkerboard[i, j] == checkerboard[i + 1, j - 1] & checkerboard[i, j] == checkerboard[i + 2, j - 2] & checkerboard[i, j] == checkerboard[i + 3, j - 3] & checkerboard[i, j] != ' ')
                            {
                                win = true;
                                return win;
                            }
                            else
                            {
                                win = false;
                                break;
                            }

                        case 4:
                            break;

                        default:
                            Console.WriteLine("發生例外狀況");
                            break;



                    }
                }
            }
            return win;
        }

        private static void piece_down(ref char[,] checkerboard, char put_piece, int player_put_position) //落下玩家棋子
        {
            int Row = 0;
            for (int i = 0; i < checkerboard.GetUpperBound(0) + 1; i++)
            {
                if (checkerboard[i, player_put_position] != ' ')
                {
                    Row = Row + 1;
                }
            }
            checkerboard[5 - Row, player_put_position] = put_piece;

        }

        private static void show_board(char[,] checkerboard) // 展示目前棋盤
        {
            Console.WriteLine("  1   2   3   4   5   6   7  ");
            for (int i = 0; i < checkerboard.GetUpperBound(0) + 1; i++)
            {
                Console.WriteLine("+---+---+---+---+---+---+---+");
                Console.Write("|");
                for (int j = 0; j < checkerboard.GetUpperBound(1) + 1; j++)
                {
                    Console.Write(" {0} |", checkerboard[i, j]);
                }
                Console.WriteLine("");
            }
            Console.WriteLine("+---+---+---+---+---+---+---+");
            Console.WriteLine("  1   2   3   4   5   6   7  ");
        }

        private static bool check_board(char[,] checkerboard, int player_put_position) // 確認棋盤有無空格
        {
            bool isRowFull = false;
            int Row = 0;
            for (int i = 0; i < checkerboard.GetUpperBound(0) + 1; i++)
            {
                if (checkerboard[i, player_put_position] != ' ')
                {
                    Row = Row + 1;
                }
            }
            if (Row == 6)
            {
                isRowFull = true;
            }
            return isRowFull;
        }
        private static bool check_input(int player_put_position) // 確認玩家輸入
        {
            bool is_input_correct = false;
            for (int i = 0; i < 7; i++)
            {
                if (i == player_put_position)
                {
                    is_input_correct = true;
                    return is_input_correct;
                }
            }
            return is_input_correct;
        }
    }
}
