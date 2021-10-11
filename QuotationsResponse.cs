using System;


namespace ItemHeldDelay
{
	public class Quotation
	{
		public String _id { get; set; }

		// The quotation text
		public String content { get; set; }

		// The full name of the author
		public String author { get; set; }

		// The `slug` of the quote author
		public String authorSlug { get; set; }

		// The length of quote (number of characters)
		public int length { get; set; }

		// An array of tag names for this quote
		public String[] tags { get; set; }

		public String dateAdded { get; set; }

		public String dateModified { get; set; }

	}

	internal class QuotationsResponse
	{
		// The number of quotes returned in this response
		public int count { get; set; }

		// The total number of quotes matching this query
		public int totalCount { get; set; }

		// The current page number
		public int page { get; set; }

		// The total number of pages matching this request
		public int totalPages { get; set; }

		// The 1-based index of the last result included in the current response.
		public int lastItemIndex { get; set; }

		// The array of quotes
		public Quotation[] results { get; set; }
	}
}
