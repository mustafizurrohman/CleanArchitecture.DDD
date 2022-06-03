﻿namespace CleanArchitecture.DDD.Application.Services;

public interface IEDCMSyncService
{
    Task<IEnumerable<DoctorDTO>> SyncDoctors();
    void SyncDoctorsInBackground();
    Task<IEnumerable<DoctorCityDTO>> SyncDoctorsWithSomeInvalidData();
}