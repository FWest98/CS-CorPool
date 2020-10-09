import { Vehicle } from './Vehicle';

export class User {
    constructor(
      public id: string,
      public tenantId: string,
      public userName: string,
      public normalizedUserName: string,
      public fullName: string,
      public email: string,
      public normalizedEmail: string,
      public emailConfirmed: boolean,
      public passwordHash: string,
      public securityStamp: string,
      public phoneNumber: string,
      public phoneNumberConfirmed: boolean,
      public vehicles: Vehicle[]
    ) {}
  }
  