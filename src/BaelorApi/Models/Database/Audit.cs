using System;
using System.ComponentModel.DataAnnotations;

namespace BaelorApi.Models.Database
{
	public class Audit
	{
		public Audit()
		{
			UpdatedAt = CreatedAt = DateTime.UtcNow;
		}

		[Key]
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}
}