using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Recipes_api.Models
{
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get;set;}

        [Column(TypeName = "nvarchar(100)")]
        public string? Title {get; set;}

        [Column(TypeName = "nvarchar(100)")]
        public string? Cuisine  {get;set;}

        [Column(TypeName = "nvarchar(500)")]
        public string? Instructions {get;set;} 

        // In the following code, `IngredientsJson` stores the serialised form of `Ingredients` list in the database. However, its value is not shown in swagger UI as we have mentioned it as   [System.Text.Json.Serialization.JsonIgnoreAttribute] attribute. On the other hand, `Ingredients` is the JSON format list but its value is not mapped on the database. 
       
       [System.Text.Json.Serialization.JsonIgnoreAttribute] 
        public string? IngredientsJson { get; set; } // Temporary parameter to store the JSON values in string format in database

        [NotMapped]
        public List<Ingredient>? Ingredients
        {
            get => string.IsNullOrEmpty(IngredientsJson)? new List<Ingredient>(): JsonConvert.DeserializeObject<List<Ingredient>>(IngredientsJson);
            set => IngredientsJson = JsonConvert.SerializeObject(value);
        }
        
    }
    public class Ingredient
    {
        public string? name {get;set;}
        public string? quantity {get;set;}
    }
}