﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPLBlazor.API.Data;
using WPLBlazor.API.Filters;
using WPLBlazor.API.Models;

namespace WPLBlazor.API.Controllers
{
#if !DEBUG
    [APIKey]
#endif
    [Route("/[controller]")]
    [ApiController]
    public class TeamDetailsController(WPLStatsDbContext context) : ControllerBase
    {
        private readonly WPLStatsDbContext _context = context;

        // GET: api/TeamDetails
        [HttpGet]
        [ResponseCache(Duration = 40, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<ActionResult<IEnumerable<TeamDetails>>> GetTeamDetails()
        {
            if (_context.TeamDetails == null)
            {
                return NotFound();
            }
            return await _context.TeamDetails.ToListAsync();
        }

        // GET: api/TeamDetails/5
        [HttpGet("{id}")]
        [ResponseCache(Duration = 40, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<ActionResult<TeamDetails>> GetTeamDetail(int id)
        {
            if (_context.TeamDetails == null)
            {
                return NotFound();
            }
            var teamDetail = await _context.TeamDetails.FindAsync(id);

            if (teamDetail == null)
            {
                return NotFound();
            }

            return teamDetail;
        }

        // PUT: api/TeamDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeamDetail(int id, TeamDetails teamDetail)
        {
            if (id != teamDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(teamDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TeamDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TeamDetails>> PostTeamDetail(TeamDetails teamDetail)
        {
            if (_context.TeamDetails == null)
            {
                return Problem("Entity set 'WPLStatsDbContext.TeamDetails'  is null.");
            }
            if (!TeamDetailExists(teamDetail.Id))
            {
                _context.TeamDetails.Add(teamDetail);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.Update(teamDetail);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetTeamDetail", new { id = teamDetail.Id }, teamDetail);
        }

        // DELETE: api/TeamDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeamDetail(int id)
        {
            if (_context.TeamDetails == null)
            {
                return NotFound();
            }
            var teamDetail = await _context.TeamDetails.FindAsync(id);
            if (teamDetail == null)
            {
                return NotFound();
            }

            _context.TeamDetails.Remove(teamDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeamDetailExists(int id)
        {
            return (_context.TeamDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
