﻿using Microsoft.EntityFrameworkCore;
using SmE_CommerceModels.DBContext;
using SmE_CommerceModels.Enums;
using SmE_CommerceModels.Models;
using SmE_CommerceModels.ReturnResult;
using SmE_CommerceRepositories.Interface;

namespace SmE_CommerceRepositories;

public class AddressRepository(SmECommerceContext defaultdbContext) : IAddressRepository
{
    public async Task<Return<IEnumerable<Address>>> GetAddressesByUserIdAsync(Guid userId)
    {
        try
        {
            var result = await defaultdbContext.Addresses
                .Where(x => x.UserId == userId && x.Status != GeneralStatus.Deleted)
                .ToListAsync();

            return new Return<IEnumerable<Address>>
            {
                Data = result,
                IsSuccess = true,
                Message = result.Count > 0 ? SuccessfulMessage.Found : ErrorMessage.NotFound,
                TotalRecord = result.Count
            };
        }
        catch (Exception ex)
        {
            return new Return<IEnumerable<Address>>
            {
                Data = null,
                IsSuccess = false,
                Message = ErrorMessage.InternalServerError,
                InternalErrorMessage = ex,
                TotalRecord = 0
            };
        }
    }

    public async Task<Return<Address>> GetAddressByIdAsync(Guid addressId)
    {
        try
        {
            var result = await defaultdbContext.Addresses
                .Where(x => x.AddressId == addressId && x.Status != GeneralStatus.Deleted)
                .FirstOrDefaultAsync();

            return new Return<Address>
            {
                Data = result,
                IsSuccess = true,
                Message = result != null ? SuccessfulMessage.Found : ErrorMessage.NotFound
            };
        }
        catch (Exception ex)
        {
            return new Return<Address>
            {
                Data = null,
                IsSuccess = false,
                Message = ErrorMessage.InternalServerError,
                InternalErrorMessage = ex
            };
        }
    }

    public async Task<Return<bool>> AddAddressAsync(Address address)
    {
        try
        {
            await defaultdbContext.Addresses.AddAsync(address);
            await defaultdbContext.SaveChangesAsync();

            return new Return<bool>
            {
                Data = true,
                IsSuccess = true,
                Message = SuccessfulMessage.Created
            };
        }
        catch (Exception ex)
        {
            return new Return<bool>
            {
                Data = false,
                IsSuccess = false,
                Message = ErrorMessage.InternalServerError,
                InternalErrorMessage = ex
            };
        }
    }

    public async Task<Return<Address>> UpdateAddressAsync(Address address)
    {
        try
        {
            defaultdbContext.Addresses.Update(address);
            await defaultdbContext.SaveChangesAsync();

            return new Return<Address>
            {
                Data = address,
                IsSuccess = true,
                Message = SuccessfulMessage.Updated
            };
        }
        catch (Exception ex)
        {
            return new Return<Address>
            {
                Data = null,
                IsSuccess = false,
                Message = ErrorMessage.InternalServerError,
                InternalErrorMessage = ex
            };
        }
    }

    public async Task<Return<bool>> DeleteAddressAsync(Guid addressId)
    {
        try
        {
            var address = await defaultdbContext.Addresses
                .Where(x => x.AddressId == addressId)
                .FirstOrDefaultAsync();

            if (address == null)
            {
                return new Return<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    Message = ErrorMessage.NotFound
                };
            }

            address.Status = GeneralStatus.Deleted;
            defaultdbContext.Addresses.Update(address);
            await defaultdbContext.SaveChangesAsync();

            return new Return<bool>
            {
                Data = true,
                IsSuccess = true,
                Message = SuccessfulMessage.Deleted
            };
        }
        catch (Exception ex)
        {
            return new Return<bool>
            {
                Data = false,
                IsSuccess = false,
                Message = ErrorMessage.InternalServerError,
                InternalErrorMessage = ex
            };
        }
    }

    public async Task<Return<bool>> SetDefaultAddressAsync(Guid addressId)
    {
        try
        {
            var addresses = await defaultdbContext.Addresses
                .ToListAsync();

            foreach (var address in addresses)
            {
                address.IsDefault = address.AddressId == addressId;
                defaultdbContext.Addresses.Update(address);
            }

            await defaultdbContext.SaveChangesAsync();

            return new Return<bool>
            {
                Data = true,
                IsSuccess = true,
                Message = SuccessfulMessage.Successfully
            };
        }
        catch (Exception ex)
        {
            return new Return<bool>
            {
                Data = false,
                IsSuccess = false,
                Message = ErrorMessage.InternalServerError,
                InternalErrorMessage = ex
            };
        }
    }

    public async Task<Return<Address>> GetDefaultAddressAsync(Guid userId)
    {
        try
        {
            var address = await defaultdbContext.Addresses
                .Where(x => x.UserId == userId && x.IsDefault == true && x.Status != GeneralStatus.Deleted)
                .FirstOrDefaultAsync();

            if (address == null)
            {
                return new Return<Address>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = ErrorMessage.NotFound
                };
            }

            return new Return<Address>
            {
                Data = address,
                IsSuccess = true,
                Message = SuccessfulMessage.Found
            };
        } catch (Exception ex)
        {
            return new Return<Address>
            {
                Data = null,
                IsSuccess = false,
                Message = ErrorMessage.InternalServerError,
                InternalErrorMessage = ex
            };
        }
    }
}
