import { Offer } from './Offer';
import { User } from './User';

export class Confirmation {
    constructor(
      public Id: string,
      public offer: Offer,
      public user: User,
      public pickUpPoint: Location,
    ) {}
  }

