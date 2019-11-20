using System;
using System.Collections.Generic;
using System.Text;

namespace Card_War.Models
{
    public class Deck
    {
        private List<Card> cards = new List<Card>();


        /// <summary>
        /// Creates a new deck, full for full 52 card deck, else empty deck
        /// </summary>
        public Deck(string value)
        {
            if (value == "full")
            {
                string[] suits = new string[] { "H", "D", "S", "C" };

                foreach (string suit in suits) // Outside loop cycles through each card suit
                {
                    for (int rank = 2; rank <= 14; rank++) // Inside loop cycles through each rank
                    {
                        Card card = new Card(rank, suit, false);
                        cards.Add(card);
                    }
                }
            }
        }


        /// <summary>
        /// Add a new card to the deck
        /// </summary>
        /// <param name="card"></param>
        public void AddOne(Card card)
        {
            cards.Add(card);
        }

        public Card DealOne()
        {
            Card result = null;

            if (cards.Count > 0)
            {
                result = cards[0];
                cards.RemoveAt(0);
            }
            return result;
        }

        public List<Card> Deal(int count)
        {
            List<Card> hand = new List<Card>();

            if (cards.Count >= count)
            {
                for (int i = 1; i <= count; i++)
                {
                    hand.Add(cards[0]);
                    cards.RemoveAt(0);
                }
            }
            return hand;
        }

        public void Shuffle()
        {
            List<Card> shuffledCards = new List<Card>();

            Random random = new Random();
            while (cards.Count > 0)
            {
                int randomIndex = random.Next(cards.Count);
                shuffledCards.Add(cards[randomIndex]);
                cards.RemoveAt(randomIndex);
            }
            cards = shuffledCards;
        }
        
        public int Count()
        {
            return cards.Count;
        }
    }
}
