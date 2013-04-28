
namespace WebDriverModels.Tests.Models
{
	[ModelLocator("container")]
	public class InputModel
	{
		[ModelLocator("textField")]
		public virtual string Text { get; set; }

		[ModelLocator("passwordField")]
		public virtual string Password { get; set; }

		[ModelLocator("emailField")]
		public virtual string Email { get; set; }

		[ModelLocator("textarea")]
		public virtual string TextArea { get; set; }

		[ModelLocator("checkbox")]
		public virtual bool CheckboxIsChecked { get; set; }

		[ModelLocator("radioGroup")]
		public virtual string RadioGroupSelectedValue { get; set; }

		[ModelLocator("select")]
		public virtual string DropdownListSelectedValue { get; set; }

		[ModelLocator("inputButton")]
		public virtual void ClickButton()
		{
		}

		[ModelLocator("inputButtonMessage")]
		public virtual string InputButtonMessage { get; set; }
	}
}
