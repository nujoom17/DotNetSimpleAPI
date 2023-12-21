// Interface
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WebApplicationTest.Entities;
using WebApplicationTest.Mappings;
using WebApplicationTest.Models;


// Repository
public class CompanyRepository : ICompanyRepository
{
    private readonly DbContextMain _context;
    public readonly CompanyMapping _CompanyMapping;

    public CompanyRepository(DbContextMain context, CompanyMapping companyMapping)
    {
        _context = context;
        _CompanyMapping = companyMapping;
    }


    public IEnumerable<CompanyModel> GetAllCompanies()
    {
        var CompanyModels = _context.Companies.Select(CompanyEntity => _CompanyMapping.MapCompanyToCompanyModel(CompanyEntity)).ToList();

        return CompanyModels;
    }

    public CompanyModel GetCompanyById(Guid guid)
    {
        var Company = _context.Companies.Select(CompanyEntity => _CompanyMapping.MapCompanyToCompanyModel(CompanyEntity)).
            FirstOrDefault(x => x.Guid == guid);

        if (Company == null)
        {
            throw new Exception("Company profile not found");
        }

        return Company;
    }

    public async Task<CompanyModel> EditCompany(CompanyDTO payloadData)
    {
        var existingCompany = _context.Companies.FirstOrDefault(x => x.Guid == payloadData.Guid);

        if (existingCompany == null || payloadData?.Guid == null)
        {
            throw new Exception("The Company corresponding to the unique identifier is non-existent");
        }

        foreach (PropertyInfo property in payloadData.GetType().GetProperties())
        {
            if (property.PropertyType == typeof(Guid))
            {
                continue;
            }
            else
            {
                var newValue = property.GetValue(payloadData);

                // Update the property if the new value is not null
                if (newValue != null)
                {
                    var existingProperty = existingCompany.GetType().GetProperty(property.Name);
                    existingProperty?.SetValue(existingCompany, newValue);
                }
            }
        }

        existingCompany.UpdatedDate = DateTime.UtcNow;

        // Save changes to the database
        await _context.SaveChangesAsync();

        return GetCompanyById((Guid)existingCompany.Guid);
    }


    public async Task<CompanyModel> CreateCompany(CompanyDTO payloadData)
    {

        string key = $"genTemplateCompany@{_context.Companies.Count()}";

        // Generate a new GUID with the key
        Guid? newGuid = null;
        Company CompanyExisting = _context.Companies.FirstOrDefault(x => x.Guid == payloadData.Guid);


        if (CompanyExisting is not null)
        {
            newGuid = (Guid)payloadData.Guid;
        }
        else
        {
            newGuid = GenerateGuidWithKey(key);
        }


        Company newCompany = new Company
        {
            Name = payloadData.Name?.Trim(),
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            Guid = newGuid,
            LocationId = (int)payloadData.LocationId
        };

        if(newCompany.LocationId==null && (_context.MSTR_Locations.FirstOrDefault(x => x.coordinates.Trim() == payloadData.LocationCoords || x.Name.Trim() == payloadData.LocationName) == null)){
            throw new Exception("Location is invalid or non-existent");
        } 

     
        if (CompanyExisting is not null)
        {
            newCompany.Id = CompanyExisting.Id;
            _context.Entry(CompanyExisting).State = EntityState.Detached;
            newCompany.Guid = GenerateGuidWithKey(key);
            _context.Companies.Update(newCompany);
        }
        else
        {
            _context.Companies.Add(newCompany);
        }

        return GetCompanyById((Guid)newGuid);

    }

    private Guid GenerateGuidWithKey(string key)
    {
        // Use key to influence GUID generation
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        Guid guidWithKey = Guid.NewGuid();
        byte[] guidBytes = guidWithKey.ToByteArray();

        for (int i = 0; i < keyBytes.Length; i++)
        {
            guidBytes[i % 16] ^= keyBytes[i];
        }

        return new Guid(guidBytes);
    }


    public bool DeleteCompany(Guid guidParsed)
    {
        var Company = _context.Companies.FirstOrDefault(x => x.Guid == guidParsed);

        if (Company == null)
        {
            return false;
        }

        // Remove the Company from the database
        _context.Companies.Remove(Company);
        _context.SaveChanges();

        return true;
    }

    public CompanyModel AddUserToCompany(Guid CompanyGuid)
    {
        return new CompanyModel();
    }

}


public interface ICompanyRepository
{
    IEnumerable<CompanyModel> GetAllCompanies();

    CompanyModel GetCompanyById(Guid id);

    Task<CompanyModel> CreateCompany(CompanyDTO payload);

    CompanyModel AddUserToCompany(Guid CompanyGuid);

    bool DeleteCompany(Guid guid);
    Task<CompanyModel> EditCompany(CompanyDTO payload);
}