using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvertNotificator.Models
{
	public class BaseEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public Guid Id { get; set; }

		[Required, Column(TypeName = "datetime2"), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime CreatedDate { get; set; }
	}
}