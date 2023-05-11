﻿using Microsoft.AspNetCore.Mvc;

namespace BlazorFullStackCrud.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuperHeroController : ControllerBase
{
    public static List<Comic> comics = new()
    {
        new Comic(){ Id = 1, Name = "Marvel"},
        new Comic(){ Id = 2, Name = "DC"},
    };

    public static List<SuperHero> heroes = new()
    {
        new SuperHero(){ Id = 1, FirstName = "Peter", LastName="Parker", HeroName ="Spider-man", Comic = comics[0]},
        new SuperHero(){ Id = 2, FirstName = "Bruce", LastName="Wayne", HeroName ="Batman", Comic = comics[1]}
    };

    [HttpGet]
    public async Task<ActionResult<List<SuperHero>>> GetSuperHeroes()
    {
        return Ok(heroes);
    }

    [HttpGet]
    [Route("{id}")] // or [[HttpGet"{id}"]]
    public async Task<ActionResult<SuperHero>> GetSingleHero(int id)
    {
        var hero = heroes.FirstOrDefault(h => h.Id == id);
        if (hero == null)
        {
            return NotFound("Sorry, no hero here. :/");
        }
        return Ok(hero);
    }
}