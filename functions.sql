create function check_parking_validity(@efz_id int, @station_id int)
returns bit 
as
begin
declare @kapazität int;
declare @besetztung int;
select @kapazität = kapazität from Station where station_id = @station_id;
select @besetztung = count(fk_station_id) from E-Fahrzeuge where fk_station_id = @station_id;

if(@besetztung < @kapazität)
	return 1;
return 0;
end

create function 



	