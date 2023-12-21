using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using Recipes_api.Models;
using Microsoft.EntityFrameworkCore;


namespace Recipes_api.Controllers
{
    [ApiController]
    // api will be available at this route
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        // Task 5: add solution here
 private RecipeContext _recipeContext;
        
        public RecipesController(RecipeContext context){
            _recipeContext = context;
        }

         /// <summary>
         /// Post a new recipe
         /// </summary>
         [HttpPost]
         public async Task<ActionResult<Recipe>> PostRecipes(Recipe recipe)
         {
             _recipeContext.Recipes.Add(recipe);
             await _recipeContext.SaveChangesAsync();
             string url = "/api/recipes/" + recipe.Id;
             return CreatedAtAction(null, null, recipe, "The resource has been created successfully at "+ url);
         }       
        // Task 6: add solution here
        ///summary
        ///Retrieve all recipes
        ///summary 
        [HttpGet]
        public ActionResult<IEnumerable<Recipe>> GetRecipes()
        {
            var recipes = _recipeContext.Recipes.ToList();
            return Ok(recipes);
        }
        // Task 7: add solution here
      /// <summary>
        /// Search recipes by ingredients
        /// </summary>
        [HttpGet("/recipes/searchByIngredients/{ingredient}")]
        public ActionResult<IEnumerable<Recipe>> GetFilterRecipesByIngredients(string ingredient)
        {
            var recipes = _recipeContext.Recipes
            .AsEnumerable()
            .Where(
                recipe =>
                {
                    var ingredientsList = string.IsNullOrEmpty(recipe.IngredientsJson) ? new List<Ingredient>() : JsonSerializer.Deserialize<List<Ingredient>>(recipe.IngredientsJson, new JsonSerializerOptions());
                    return ingredientsList != null && ingredientsList.Any(ingred => ingred.name == ingredient);
                }
                )
            .ToList();
            if (!recipes.Any()) { return NotFound(); }
            return Ok(recipes);
        }
        // Task 8: add solution here
  /// <summary>
       /// Search recipes by cuisine
       /// </summary>
       [HttpGet("/recipes/searchByCuisine/{cuisine}")]
       public ActionResult<IEnumerable<Recipe>> GetFilterRecipesByCuisine(string cuisine)
       {
           var recipes = _recipeContext.Recipes.Where(s => s.Cuisine != null && s.Cuisine.Contains(cuisine)).ToList();
           if (!recipes.Any()) { return NotFound(); }
           return Ok(recipes);
       }
        // Task 9: add solution here
     /// <summary>
       /// Search a recipe by its title.
       /// </summary>
       [HttpGet("/recipes/searchByTitle/{title}")]
       public ActionResult<IEnumerable<Recipe>> GetFilterRecipesByTitle(string title)
       {
           var recipes = _recipeContext.Recipes.Where(s => s.Title != null && s.Title.Contains(title)).ToList();
           if (!recipes.Any()) { return NotFound(); }
           return Ok(recipes);
       }
        // Task 10: add solution here
  /// <summary>
       /// Delete a recipe
       /// </summary>
       [HttpDelete("{id}")]
       public async Task<IActionResult> DeleteRecipe(int id)
       {
           var recipe = await _recipeContext.Recipes.FindAsync(id);
           if (recipe == null)
           {
               return NotFound();
           }

           _recipeContext.Recipes.Remove(recipe);
           await _recipeContext.SaveChangesAsync();
           return Ok("Recipe has been deleted successfully");
       }
        // Task 11: add solution here
       /// <summary>
       /// Update a recipe
       /// </summary>
       [HttpPut("{id}")]
       public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
       {
           if (id != recipe.Id)
           {
               return StatusCode(400, "The id parameter should be equal to the id of the new data of the object to be updated");
           }
           if (await _recipeContext.Recipes.FindAsync(id) == null)
           {
               return StatusCode(404, "The recipe you want to update is not found in the database");
           }
           _recipeContext.Entry(recipe).CurrentValues.SetValues(recipe);
           await _recipeContext.SaveChangesAsync();           
           return Ok("Recipe has been updated successfully");
       }
    }
}