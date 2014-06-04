WebDriverModels
===============

Adding Full/Partial Page Model support for WebDriver, intended for use with automated testing.


## Basic Usage

Ensure that when you create your IWebDriver, it is assigned to `CurrentDriver.Driver` (I intend to break this dependency soon). For example:
```csharp
IWebDriver driver = CurrentDriver.Driver = new FirefoxDriver();
```

Assuming the following HTML:

```html
<div id="idSelector">
  <label>First Name: <input type="text" class="classSelector" /></label>
  <input type="submit" class="cssSelector" value="Submit" />
</div>
```

Create a model along these lines:

```csharp
[ModelLocator("idSelector")
public class MyModel {
  [ModelLocator(Method = How.ClassName, Identifier = "classSelector")
  public virtual string MyFirstName { get;set; }
  [ModelLocator(Method = How.CssSelector, Identifier = ".cssSelector")
  public virtual void Submit { }
}
```

In your code where you need to interact with the page:

```csharp
MyModel model = CurrentDriver.Driver.FindModel<MyModel>();

model.MyFirstName = "John";
model.Submit();
```

## Key Points

* All properties and methods that you wish to attach to must be `virtual`

## Supports

* Static text (property type: string) - this simply reads the innerText of the element
* Text, textarea, password, email input fields (property type: string)
* Checkboxes (property type: bool)
* Anything that is normally clickable (buttons, <a> tags etc) (method)
* Sub Models (property type: Class with a `[ModelLocator]` attribute)
* Collections of static text elements (property type: List<string>)
* Collections of sub models (property type List<T> where T is a class with a `[ModelLocator` attribute)

## Methods

Note: Most methods can be called either by the format `ModelFinder.FindModel<MyModel>(driver)` or by `driver.FindModel<MyModel>()`

* `MyModel model = driver.FindModel<MyModel>`
* `MyModel model = driver.WaitForModel<MyModel>(TimeSpan.FromSeconds(5))`
* `Assert.IsTrue(driver.ModelExists<MyModel>())`
* `Assert.IsTrue(driver.ModelExists<MyModel>(TimeSpan.FromSeconds(5))`
* `Assert.IsTrue(driver.ModelPropertyExists<MyModel>(m => m.MyFirstName)`
* `Assert.IsTrue(driver.ModelPropertyExists<MyModel>(m => m.Submit())`
