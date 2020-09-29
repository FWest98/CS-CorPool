import { Confirmation } from './Confirmation';
import { Location } from './Location';
import { User } from './User';
import { Vehicle } from './Vehicle';

export class Offer {
  constructor(
    public Id: string,
    public vehicle: Vehicle,
    public from: Location,
    public to: Location,
    public user: User,
    public confirmations: Confirmation[],
    public arrivalTime: Date,
  ) {}
}
