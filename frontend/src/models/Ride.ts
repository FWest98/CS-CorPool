export class Ride {
  constructor(
    public date: string,
    public from: string,
    public to: string,
    public spotsAvailable: number,
    public capacity: number,
  ) {}
}
