using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

public class Test
{
	private class Card
	{
		public string SourceId { get; set; }
		public string DestinationId { get; set; }
	}
	
	private class Item
	{
		public Card Card { get;set; }
		public Item Next { get;set; }
	}
	
	private class Queue
	{
		public Item First { get;set; }
		public Item Last { get;set; }
	}
	
	private interface IRepository<T>
	{
		void Add(T obj);
		void Update(T obj);
		void Delete(T obj);
		IEnumerable<T> GetAll();
	}
	
	private class CardRepository : IRepository<Card>
	{
		private readonly IList<Queue> chains;
		
		public CardRepository()
		{
			chains = new List<Queue>();
		}
		
		public void Update(T obj)
		{
			throw new NotImplementedException();
		}
		public void Delete(T obj)
		{
			throw new NotImplementedException();
		}
		
		public void Add(Card card)
		{
			if (card == null)
				throw new ArgumentNullException("card");
			int queueId = -1;
			for (int i = 0; i < chains.Count; i++)
			{
				var queue = chains[i];
				if (card.DestinationId == queue.First.Card.SourceId)
				{
					if (queueId < 0)
					{
					    var cardItem = new Item { Card = card };
						var tmpItem = queue.First;
						queue.First = cardItem;
						cardItem.Next = tmpItem;
						queueId = i;
					}
					else
					{
						chains[queueId].Last.Next = queue.First;
						chains[queueId].Last = queue.Last; 
						chains.Remove(queue);
					}
				}
				if (queue.Last.Card.DestinationId == card.SourceId)
			    {
			    	if (queueId < 0)
			    	{
			    		var cardItem = new Item { Card = card };
		    			queue.Last.Next = cardItem;
		    			queue.Last = cardItem;
		    			queueId = i;
			    	}
			    	else
			    	{
			    		var tmpItem = chains[queueId].First;
			    		chains[queueId].First = queue.First;
			    		queue.Last.Next = tmpItem;
			    		chains.Remove(queue);
			    	}
			    }
			}
			if (queueId < 0)
			{
				var cardItem = new Item { Card = card };
				chains.Add(new Queue(){ First = cardItem, Last = cardItem });
			}
		}
		
		public IEnumerable<Card> GetAll()
		{
			foreach (var chain in chains)
			{
				var item = chain.First;
				while (item != null)
				{
					yield return item.Card;
					item = item.Next;
				}
			}
		}
	}
	
	public static void Main()
	{
		var list = new List<Card>() { 
			new Card() { 
				SourceId = "Мельбурн",
				DestinationId = "Кельн" 
			},
			new Card() {
				SourceId = "Москва",
				DestinationId = "Париж" 
			},
			new Card() {
				SourceId = "Кельн",
				DestinationId = "Москва"
			}
		};
		var repository = new CardRepository();
		foreach (var card in list)
		{
			repository.Add(card);
		}
		var cards = repository.GetAll().ToArray();
		Debug.Assert(cards.Length == 3);
		Debug.Assert(cards[0].SourceId == "Мельбурн");
		Debug.Assert(cards[0].DestinationId == "Kельн");
		Debug.Assert(cards[1].SourceId == "Kельн");
		Debug.Assert(cards[1].DestinationId == "Москва");
		Debug.Assert(cards[2].SourceId == "Москва");
		Debug.Assert(cards[2].DestinationId == "Париж");
	    // The complexity of the sort algorithm would be measured as O(n + k) = O(n)
	}
}