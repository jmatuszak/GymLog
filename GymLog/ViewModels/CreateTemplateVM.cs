using GymLog.Models;

namespace GymLog.ViewModels
{
	public class CreateTemplateVM
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<OrderedSetList>? OrderedSetsLists { get; set; }
	}
}
