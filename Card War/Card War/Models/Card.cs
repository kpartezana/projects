using System;
using System.Collections.Generic;
using System.Text;

namespace Card_War.Models
{
    public class Card
    {
        /// <summary>
        /// Creates new card object
        /// </summary>
        /// <param name="rank">Value of card from 2 to 14(Ace)</param>
        /// <param name="suit">Suit of card</param>
        public Card(int rank, string suit)
        {
            this.Rank = rank;
            this.Suit = suit;
            IsFaceUp = false;
        }

        /// <summary>
        /// Creates new card object and lets you specify face up or face down
        /// </summary>
        /// <param name="rank">Value of card from 2 to 14(Ace)</param>
        /// <param name="suit">Suit of card</param>
        /// <param name="isFaceUp">True if face is showing</param>
        public Card(int rank, string suit, bool isFaceUp)
        {
            this.Rank = rank;
            this.Suit = suit;
            IsFaceUp = isFaceUp;
        }

        /// <summary>
        /// Value of the card from 2 to 14(Ace)
        /// </summary>
        public int Rank { get; set; }

        private string suit;
        /// <summary>
        /// "Heart symbol", "Diamond symbol", "Spade symbol", or "Club symbol"
        /// </summary>

        public string Suit
        {
            get
            {
                return suit;
            }
            set
            {
                if (value.ToLower() == "h") // Hearts
                {
                    suit = "H";
                    // suit = "\u2665";
                }
                else if (value.ToLower() == "d") // Diamonds
                {
                    suit = "D";
                    // suit = "\u2666";
                }
                else if (value.ToLower() == "s") // Spade
                {
                    suit = "S";
                    // suit = "\u2660";
                }
                else if (value.ToLower() == "c") // Club
                {
                    suit = "C";
                    // suit = "\u2663"; 
                }
                else
                {
                    // We did not get a valid value
                    throw new ArgumentException("Invalid suit value");
                }
            }
        }

        /// <summary>
        /// True if the face of the card is showing
        /// </summary>
        public bool IsFaceUp { get; set; }

        /// <summary>
        /// Get display character for rank
        /// </summary>
        public string Title
        {
            get
            {
                string title = "";
                if (Rank > 1 && Rank < 11)
                {
                    title += Rank.ToString();
                }
                else if (Rank == 14)
                {
                    title += "A";
                }
                else if (Rank == 13)
                {
                    title += "K";
                }
                else if (Rank == 12)
                {
                    title += "Q";
                }
                else if (Rank == 11)
                {
                    title += "J";
                }
                title += Suit;
                return title;
            }
        }

    }
}
