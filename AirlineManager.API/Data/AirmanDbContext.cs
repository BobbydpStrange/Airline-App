using System;
using System.Collections.Generic;
using AirlineManager.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace AirlineManager.API.Data;

public partial class AirmanDbContext : DbContext
{
    public AirmanDbContext()
    {
    }

    public AirmanDbContext(DbContextOptions<AirmanDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Airplane> Airplanes { get; set; }

    public virtual DbSet<AirplaneType> AirplaneTypes { get; set; }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<CertifiedMechanic> CertifiedMechanics { get; set; }

    public virtual DbSet<CertifiedPilot> CertifiedPilots { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<FlightTimeTemplate> FlightTimeTemplates { get; set; }

    public virtual DbSet<Hanger> Hangers { get; set; }

    public virtual DbSet<HangerType> HangerTypes { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Shared.Data.Maintenance> Maintenances { get; set; }

    public virtual DbSet<MaintenanceCertification> MaintenanceCertifications { get; set; }

    public virtual DbSet<MaintenanceType> MaintenanceTypes { get; set; }

    public virtual DbSet<OfferedFlight> OfferedFlights { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PlanesDueForMaintenance> PlanesDueForMaintenances { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airplane>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("airplane_pkey");

            entity.ToTable("airplane", "airman");

            entity.HasIndex(e => e.TailNumber, "airplane_tail_number_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('airplane_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AirplaneTypeId).HasColumnName("airplane_type_id");
            entity.Property(e => e.TailNumber).HasColumnName("tail_number");

            entity.HasOne(d => d.AirplaneType).WithMany(p => p.Airplanes)
                .HasForeignKey(d => d.AirplaneTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("airplane_airplane_type_id_fkey");
        });

        modelBuilder.Entity<AirplaneType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("airplane_type_pkey");

            entity.ToTable("airplane_type", "airman");

            entity.HasIndex(e => e.Manufacturer, "airplane_type_manufacturer_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('airplane_type_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.BusinessCapacity).HasColumnName("business_capacity");
            entity.Property(e => e.CoachCapacity).HasColumnName("coach_capacity");
            entity.Property(e => e.FirstClassCapacity).HasColumnName("first_class_capacity");
            entity.Property(e => e.Manufacturer)
                .HasColumnType("character varying")
                .HasColumnName("manufacturer");
        });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("airport_pkey");

            entity.ToTable("airport", "airman");

            entity.HasIndex(e => e.AirportName, "airport_airport_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('airport_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AirportName)
                .HasColumnType("character varying")
                .HasColumnName("airport_name");
            entity.Property(e => e.LocationId).HasColumnName("location_id");

            entity.HasOne(d => d.Location).WithMany(p => p.Airports)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("airport_location_id_fkey");
        });

        modelBuilder.Entity<CertifiedMechanic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("certified_mechanic_pkey");

            entity.ToTable("certified_mechanic", "airman");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('certified_mechanic_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AirplaneTypeId).HasColumnName("airplane_type_id");
            entity.Property(e => e.CertDate).HasColumnName("cert_date");
            entity.Property(e => e.CertId).HasColumnName("cert_id");
            entity.Property(e => e.PeopleId).HasColumnName("people_id");

            entity.HasOne(d => d.AirplaneType).WithMany(p => p.CertifiedMechanics)
                .HasForeignKey(d => d.AirplaneTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("certified_mechanic_airplane_type_id_fkey");

            entity.HasOne(d => d.Cert).WithMany(p => p.CertifiedMechanics)
                .HasForeignKey(d => d.CertId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("certified_mechanic_cert_id_fkey");

            entity.HasOne(d => d.People).WithMany(p => p.CertifiedMechanics)
                .HasForeignKey(d => d.PeopleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("certified_mechanic_people_id_fkey");
        });

        modelBuilder.Entity<CertifiedPilot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("certified_pilot_pkey");

            entity.ToTable("certified_pilot", "airman");

            entity.HasIndex(e => new { e.PeopleId, e.AirplaneTypeId }, "certified_pilot_people_id_airplane_type_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('certified_pilot_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AirplaneTypeId).HasColumnName("airplane_type_id");
            entity.Property(e => e.CertDate).HasColumnName("cert_date");
            entity.Property(e => e.Expiremonths).HasColumnName("expiremonths");
            entity.Property(e => e.PeopleId).HasColumnName("people_id");

            entity.HasOne(d => d.AirplaneType).WithMany(p => p.CertifiedPilots)
                .HasForeignKey(d => d.AirplaneTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("certified_pilot_airplane_type_id_fkey");

            entity.HasOne(d => d.People).WithMany(p => p.CertifiedPilots)
                .HasForeignKey(d => d.PeopleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("certified_pilot_people_id_fkey");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("flight_pkey");

            entity.ToTable("flight", "airman");

            entity.HasIndex(e => new { e.AirplaneId, e.EstDeparture }, "flight_airplane_id_est_departure_key").IsUnique();

            entity.HasIndex(e => new { e.PilotId, e.EstDeparture }, "flight_pilot_id_est_departure_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('flight_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AirplaneId).HasColumnName("airplane_id");
            entity.Property(e => e.ArrivalTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("arrival_time");
            entity.Property(e => e.Cancelled).HasColumnName("cancelled");
            entity.Property(e => e.DepartureTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("departure_time");
            entity.Property(e => e.EstDeparture)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("est_departure");
            entity.Property(e => e.OfferedFlightId).HasColumnName("offered_flight_id");
            entity.Property(e => e.PilotId).HasColumnName("pilot_id");

            entity.HasOne(d => d.Airplane).WithMany(p => p.Flights)
                .HasForeignKey(d => d.AirplaneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("flight_airplane_id_fkey");

            entity.HasOne(d => d.OfferedFlight).WithMany(p => p.Flights)
                .HasForeignKey(d => d.OfferedFlightId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("flight_offered_flight_id_fkey");

            entity.HasOne(d => d.Pilot).WithMany(p => p.Flights)
                .HasForeignKey(d => d.PilotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("flight_pilot_id_fkey");
        });

        modelBuilder.Entity<FlightTimeTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("flight_time_template_pkey");

            entity.ToTable("flight_time_template", "airman");

            entity.HasIndex(e => e.ScheduledTime, "flight_time_template_scheduled_time_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('flight_time_template_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.ScheduledTime).HasColumnName("scheduled_time");
        });

        modelBuilder.Entity<Hanger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("hanger_pkey");

            entity.ToTable("hanger", "airman");

            entity.HasIndex(e => e.Name, "hanger_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('hanger_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.HangerType).HasColumnName("hanger_type");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.HasOne(d => d.HangerTypeNavigation).WithMany(p => p.Hangers)
                .HasForeignKey(d => d.HangerType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hanger_hanger_type_fkey");
        });

        modelBuilder.Entity<HangerType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("hanger_type_pkey");

            entity.ToTable("hanger_type", "airman");

            entity.HasIndex(e => e.Type, "hanger_type_type_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('hanger_type_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("location_pkey");

            entity.ToTable("location", "airman");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('location_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.City)
                .HasColumnType("character varying")
                .HasColumnName("city");
        });

        modelBuilder.Entity<Shared.Data.Maintenance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("maintenance_pkey");

            entity.ToTable("maintenance", "airman");

            entity.HasIndex(e => new { e.HangerId, e.Assigned }, "maintenance_hanger_id_assigned_key").IsUnique();

            entity.HasIndex(e => new { e.MechanicId, e.Assigned }, "maintenance_mechanic_id_assigned_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('maintenance_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AirplaneId).HasColumnName("airplane_id");
            entity.Property(e => e.Assigned)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("assigned");
            entity.Property(e => e.Completed)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("completed");
            entity.Property(e => e.HangerId).HasColumnName("hanger_id");
            entity.Property(e => e.MaintenanceTypeId).HasColumnName("maintenance_type_id");
            entity.Property(e => e.MechanicId).HasColumnName("mechanic_id");

            entity.HasOne(d => d.Airplane).WithMany(p => p.Maintenances)
                .HasForeignKey(d => d.AirplaneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("maintenance_airplane_id_fkey");

            entity.HasOne(d => d.Hanger).WithMany(p => p.Maintenances)
                .HasForeignKey(d => d.HangerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("maintenance_hanger_id_fkey");

            entity.HasOne(d => d.MaintenanceType).WithMany(p => p.Maintenances)
                .HasForeignKey(d => d.MaintenanceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("maintenance_maintenance_type_id_fkey");

            entity.HasOne(d => d.Mechanic).WithMany(p => p.Maintenances)
                .HasForeignKey(d => d.MechanicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("maintenance_mechanic_id_fkey");
        });

        modelBuilder.Entity<MaintenanceCertification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("maintenance_certification_pkey");

            entity.ToTable("maintenance_certification", "airman");

            entity.HasIndex(e => e.CertificationType, "maintenance_certification_certification_type_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('maintenance_certification_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CertificationType)
                .HasColumnType("character varying")
                .HasColumnName("certification_type");
            entity.Property(e => e.ExpirationMonths).HasColumnName("expiration_months");
        });

        modelBuilder.Entity<MaintenanceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("maintenance_type_pkey");

            entity.ToTable("maintenance_type", "airman");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('maintenance_type_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.EstTimeMinutes).HasColumnName("est_time_minutes");
            entity.Property(e => e.Isemergency).HasColumnName("isemergency");
            entity.Property(e => e.MaintFlightPolicy).HasColumnName("maint_flight_policy");
            entity.Property(e => e.MaintType)
                .HasColumnType("character varying")
                .HasColumnName("maint_type");
            entity.Property(e => e.RequiredCertificationId).HasColumnName("required_certification_id");

            entity.HasOne(d => d.RequiredCertification).WithMany(p => p.MaintenanceTypes)
                .HasForeignKey(d => d.RequiredCertificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("maintenance_type_required_certification_id_fkey");
        });

        modelBuilder.Entity<OfferedFlight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("offered_flight_pkey");

            entity.ToTable("offered_flight", "airman");

            entity.HasIndex(e => new { e.FromAirport, e.ToAirport }, "offered_flight_from_airport_to_airport_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('offered_flight_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.EstFlightMinutes).HasColumnName("est_flight_minutes");
            entity.Property(e => e.FromAirport).HasColumnName("from_airport");
            entity.Property(e => e.ToAirport).HasColumnName("to_airport");

            entity.HasOne(d => d.FromAirportNavigation).WithMany(p => p.OfferedFlightFromAirportNavigations)
                .HasForeignKey(d => d.FromAirport)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("offered_flight_from_airport_fkey");

            entity.HasOne(d => d.ToAirportNavigation).WithMany(p => p.OfferedFlightToAirportNavigations)
                .HasForeignKey(d => d.ToAirport)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("offered_flight_to_airport_fkey");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("people", "airman");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('people_id_seq'::regclass)");

            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");

            entity.Property(e => e.EmailPreference).HasColumnName("email_preference");

            entity.Property(e => e.EndDate).HasColumnName("end_date");

            entity.Property(e => e.Firstname)
                .HasColumnType("character varying")
                .HasColumnName("firstname");

            entity.Property(e => e.Lastname)
                .HasColumnType("character varying")
                .HasColumnName("lastname");

            entity.Property(e => e.StartDate).HasColumnName("start_date");
        });

        modelBuilder.Entity<PlanesDueForMaintenance>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("planes_due_for_maintenance", "airman");

            entity.Property(e => e.EstTimeMinutes).HasColumnName("est_time_minutes");
            entity.Property(e => e.FlightsBeforeNeeded).HasColumnName("flights_before_needed");
            entity.Property(e => e.FlightsSinceLastService).HasColumnName("flights_since_last_service");
            entity.Property(e => e.Isemergency).HasColumnName("isemergency");
            entity.Property(e => e.LastServiced)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_serviced");
            entity.Property(e => e.MaintType)
                .HasColumnType("character varying")
                .HasColumnName("maint_type");
            entity.Property(e => e.Mainttypeid).HasColumnName("mainttypeid");
            entity.Property(e => e.Manufacturer)
                .HasColumnType("character varying")
                .HasColumnName("manufacturer");
            entity.Property(e => e.Planeid).HasColumnName("planeid");
            entity.Property(e => e.TailNumber).HasColumnName("tail_number");
        });

        modelBuilder.Entity<AvalailableMechanics>(entity =>
        {
            entity.HasNoKey().ToView(null);
            entity.Property(e => e.MaintenanceType).HasColumnName("maintenance_type");
            entity.Property(e => e.CanFixPlaneType).HasColumnName("can_fix_plane_type");
            entity.Property(e => e.MechName).HasColumnName("mech_name");
            entity.Property(e => e.CertifiedMechId).HasColumnName("certified_mech_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
