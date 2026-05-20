using Microsoft.EntityFrameworkCore;
using Dystopian_Civil_Office.Data;
using Dystopian_Civil_Office.Dtos.Responses;

namespace Dystopian_Civil_Office.Services;

public class OfficeQueryService
{
    private readonly ApplicationDbContext _context;
    
    public OfficeQueryService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<PersonResponseDto>> GetPersonsAsync()
    {
        return await _context.Persons
            .AsNoTracking()
            .Select(p => new PersonResponseDto
            {
                PersonPesel = p.Pesel,
                FirstName = p.FirstName,
                MiddleName = p.MiddleName,
                LastName = p.LastName,
                Gender = p.Gender,
                BirthDate = p.BirthDate,
                BirthPlace = p.BirthPlace,
                DocumentName = p.Document != null ? p.Document.Name : null
            })
            .ToListAsync();
    }
    
    public async Task<List<DocumentResponseDto>> GetDocumentsAsync()
    {
        return await _context.Documents
            .AsNoTracking()
            .Select(d => new DocumentResponseDto
            {
                Name = d.Name,
                Category = d.Category,
                ImportDate = d.ImportDate
            })
            .ToListAsync();
    }
    
    public async Task<List<DeathRecordResponseDto>> GetDeathRecordsAsync()
    {
        return await _context.DeathRecords
            .AsNoTracking()
            .Select(dr => new DeathRecordResponseDto
            {
                RegistryNumber = dr.RegistryNumber,
                RegistryDate = dr.RegistryDate,
                PersonPesel = dr.Person.Pesel,
                DeathDate = dr.DeathDate,
                DeathPlace = dr.DeathPlace,
                CauseOfDeath = dr.CauseOfDeath,
                DocumentName = dr.Document != null ? dr.Document.Name : null
            })
            .ToListAsync();
    }
    
    public async Task<List<BirthRecordResponseDto>> GetBirthRecordsAsync()
    {
        return await _context.BirthRecords
            .AsNoTracking()
            .Select(br => new BirthRecordResponseDto
            {
                RegistryNumber = br.RegistryNumber,
                RegistryDate = br.RegistryDate,
                BornPersonPesel = br.Person.Pesel,
                MotherPesel = br.Mother != null ? br.Mother.Pesel : null,
                FatherPesel = br.Father != null ? br.Father.Pesel : null,
                BirthDate = br.Person.BirthDate,
                BirthPlace = br.Person.BirthPlace,
                DocumentName = br.Document != null ? br.Document.Name : null
            })
            .ToListAsync();
    }
    
    public async Task<List<MarriageResponseDto>> GetMarriageRecordsAsync()
    {
        return await _context.MarriageRecords
            .AsNoTracking()
            .Select(mr => new MarriageResponseDto
            {
                RegistryNumber = mr.RegistryNumber,
                RegistryDate = mr.RegistryDate,
                Spouse1Pesel = mr.Spouse1.Pesel,
                Spouse2Pesel = mr.Spouse2.Pesel,
                MarriageDate = mr.MarriageDate,
                MarriagePlace = mr.MarriagePlace,
                DocumentName = mr.Document != null ? mr.Document.Name : null
            })
            .ToListAsync();
    }
    
    public async Task<List<PersonAddressResponseDto>> GetPersonAddressesAsync()
    {
        return await _context.Persons
            .AsNoTracking()
            .Select(p => new PersonAddressResponseDto
            {
                PersonPesel = p.Pesel,
                City = p.Address.City,
                Street = p.Address.Street,
                HouseNumber = p.Address.HouseNumber,
                ApartmentNumber = p.Address.ApartmentNumber,
                PostalCode = p.Address.PostalCode,
                Country = p.Address.Country,
                DocumentName = p.Address.Document != null ? p.Address.Document.Name : null
            })
            .ToListAsync();
    }
}