using BlazorFullStackCrud.Server.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlazorFullStackCrud.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuperHeroController : ControllerBase
{
    private readonly DataContext context;

    public SuperHeroController(DataContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<SuperHero>>> GetSuperHeroes()
    {
        return Ok(await context.SuperHeroes.Include(h => h.Comic).ToListAsync());
    }

    [HttpGet("comics")]
    public async Task<ActionResult<List<Comic>>> GetComics()
    {
        return Ok(await context.Comics.ToListAsync());
    }

    [HttpGet]
    [Route("{id}")] // or [[HttpGet"{id}"]]
    public async Task<ActionResult<SuperHero>> GetSingleHero(int id)
    {
        // If not include, comic would be null
        var hero = context.SuperHeroes.Include(h => h.Comic).FirstOrDefault(h => h.Id == id);
        if (hero == null)
        {
            return NotFound("Sorry, no hero here. :/");
        }
        return Ok(hero);
    }

    [HttpPost]
    public async Task<ActionResult<List<SuperHero>>> CreateSuperHero(SuperHero hero)
    {
        hero.Comic = null;
        context.SuperHeroes.Add(hero);
        await context.SaveChangesAsync();

        return Ok(await GetDbHeroes());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(SuperHero hero, int id)
    {
        var dbHero = await context.SuperHeroes.Include(sh => sh.Comic).FirstOrDefaultAsync(sh => sh.Id == id);

        if (dbHero == null)
            return NotFound("Sorry, but not hero for you :/");

        dbHero.FirstName = hero.FirstName;
        dbHero.LastName = hero.LastName;
        dbHero.HeroName = hero.HeroName;

        await context.SaveChangesAsync();

        return Ok(await GetDbHeroes());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int id)
    {
        var dbHero = await context.SuperHeroes.Include(sh => sh.Comic).FirstOrDefaultAsync(sh => sh.Id == id);

        if (dbHero == null)
            return NotFound("Sorry, but not hero for you :/");

        context.SuperHeroes.Remove(dbHero);

        await context.SaveChangesAsync();

        return Ok(await GetDbHeroes());
    }

    private async Task<List<SuperHero>> GetDbHeroes()
    {
        return await context.SuperHeroes.Include(sh => sh.Comic).ToListAsync();
    }
}
