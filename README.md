# Kitchen Chaos
[![GitHub license](https://img.shields.io/github/license/HyagoOliveira/KitchenChaos?style=flat-square)](https://github.com/HyagoOliveira/KitchenChaos/blob/main/LICENSE)

![Kitchen Chaos Thumbnail](/Wiki/Thumbnail.png "Kitchen Chaos")

Click [here](https://youtu.be/qiwCZmpDRUY) to watch a quick gameplay session.

---

## Summary

This game is a study case for the game Overcooked! 2.

All art assets like Textures, models, audio clips etc where created by the indie game maker **Code Monkey** for this [Free Complete Unity Course](https://youtu.be/AmGSEH7QcDg). 

However, the game structure and the source-code were created by me and they are very different from the course.

This projects heavily uses custom [Unity Packages](http://34.151.243.47:4873/) created and published by me using the MIT license. You can freely use them into your project.

## How To Play

You must prepare, cook and serve up some tasty orders before time ends!

After the initial count down, Orders will be received on the screen's top left corner.
You must collect the right ingredients, prepare and plate them before delivery.

Your score will increase according in how quickly you deliver those orders.

![Kitchen Chaos Screenshot](/Wiki/Screenshot.png "Kitchen Chaos Screenshot")

Only single play is supported but you can switch between chefs to improve the kitchen dynamics.

**Play the in-game Tutorial for further instructions**.

Click [here](https://nostgames.itch.io/kitchen-chaos) to play the game on **itch.io**!

## Controls

- **Tab** - Switch between Chefs.
- **AWSD** or **Arrow Keys** - Movement.
- **Q** - Interact with Items (Cutting Table, Stove Table) 
- **E** - Interact with Plate and Ingredients (Tomato, Cheese, Bread etc)

> Gamepad is also supported.

## How To add new Recipes

1. Inside the [Recipe folder](/Assets/Settings/Recipes), create a new Recipe Data asset by using the Create menu, Kitchen Chaos > Recipes > Recipe;
2. Open the [IngredientsToRecipe prefab](/Assets/Prefabs/Recipes/IngredientsToRecipe.prefab) and link the new Recipe asset into the Recipe field;

    ![IngredientsToRecipe](/Wiki/IngredientsToRecipe.png "Ingredients To Recipe")
3. Place each child ingredient in the right position;
4. Right click on the `Prefab To Recipe` script and choose **Transfer Ingredients to Recipe**;

    ![TransferIngredientsToRecipe](/Wiki/TransferIngredientsToRecipe.png "Transfer Ingredients To Recipe")
5. Finally add this new Recipe asset int to RecipeSettings Scriptable Object.

    ![RecipeSettings](/Wiki/RecipeSettings.png "Recipe Settings")

## CI/CD

Continuous Integration and Continuous Delivery are done using [GitHub Actions for Unity](https://github.com/game-ci/unity-actions), provided by the [GameCI](https://game.ci/).

You can play the last WebGL build on [github-pages](https://hyagooliveira.github.io/KitchenChaos/).