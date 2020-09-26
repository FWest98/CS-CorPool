export class Location {
    constructor(
      public Id: string,
      public from: string,
      public to: string,
      public spotsAvailable: number,
      public capacity: number,
    ) {}
  }
  