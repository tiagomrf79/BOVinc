namespace Animal.API.Models;

public class Lactation
{
    public int Id { get; set; }
    public int LactationNumber { get; set; } //TODO: change to calculated value
    public DateOnly Calvingate { get; set; }
    public DateOnly? EndDate { get; set; }
    public Animal Animal { get; set; }

    public Lactation()
    {
        //I will only have lactations from calving events
        //So if I delete a calving event, I delete the lactation aswell

        //CalvingDate is mandatory because we always need it to calculate days in milk
        //EndDate can be null when animal is currently in lactation

        //filter lactations by animalId and order them by CalvingDate ASC
        //loop over these lactations

        //  CalvingDate cannot have a lower value than any previous lactations end date
        //  if calvingDate > current.CalvingDate and current.EndDate is not null and calvingDate < current.EndDate

        //  EndDate cannot have a higher value than any subsequent lactations calving date
        //  if endDate is not null and calvingDate < current.CalvingDate and endDate > current.CalvingDate

        //  CalvingDate cannot occur in the first 280 days following the previous lactation
        //  if calvingDate > current.CalvingDate and calvingDate < current.CalvingDate + 280 days => throw validation error

        //  CalvingDate cannot occur before the previous lactation EndDate
        //  if current.EndDate is not null and calvingDate > current.CalvingDate and calvingDate < current.EndDate => throw validation error

        //  CalvingDate cannot occur after the last 280 days of the next lactation
        //  if calvingDate < current.CalvingDate and calvingDate > current.CalvingDate - 280 days => throw validation error

        //Lactation EndDate must be updated when cow dies


    }
}
