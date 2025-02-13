/* eslint-disable no-redeclare, import/no-anonymous-default-export, unused-imports/no-unused-vars, @typescript-eslint/no-unused-vars, @typescript-eslint/no-empty-interface, @typescript-eslint/no-namespace */
import { ApiDateOnly, ApiDateTimeOffset } from "@leancodepl/api-date-datefns"
import type { ReactQueryCqrs as CQRS } from ".";

export type Query<TResult> = {}
export type Command = {}
export type Operation<TResult> = {}
export type Topic = {}

export interface Auth {
}
export interface KnownClaims {
}
export namespace KnownClaims {
    export const UserId = "sub";
    export const Role = "role";
}
export interface Roles {
}
export namespace Roles {
    export const User = "user";
    export const Admin = "admin";
}
export interface LocationDTO {
    Latitude: number;
    Longitude: number;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface AddTimeslot extends Command {
    ServiceProviderId: string;
    Date: ApiDateOnly;
    StartTime: string;
    EndTime: string;
    Price: MoneyDTO;
}
export namespace AddTimeslot {
    export const ErrorCodes = {
        ServiceProviderIdIsInvalid: 1,
        ServiceProviderDoesNotExist: 2,
        EndTimeMustBeAfterStartTime: 3,
        PriceIsNull: 4,
        TimeslotOverlapsWithExisting: 5,
        ValueCannotBeNegative: 10001,
        CurrencyIsInvalid: 10002
    } as const;
    export type ErrorCodes = typeof ErrorCodes;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface CreateServiceProvider extends Command {
    Name: string;
    Type: ServiceProviderTypeDTO;
    Description: string;
    CoverPhoto: string;
    Thumbnail: string;
    Address: string;
    Location: LocationDTO;
    Ratings: number;
}
export namespace CreateServiceProvider {
    export const ErrorCodes = {
        NameIsNullOrEmpty: 1,
        NameIsTooLong: 2,
        TypeIsNullOrInvalid: 3,
        DescriptionIsNullOrEmpty: 4,
        DescriptionIsTooLong: 5,
        CoverPhotoIsInvalid: 6,
        ThumbnailIsInvalid: 7,
        AddressIsNullOrEmpty: 8,
        AddressIsTooLong: 9,
        LocationIsNull: 10,
        LatitudeIsOutOfRange: 11001,
        LongitudeIsOutOfRange: 11002
    } as const;
    export type ErrorCodes = typeof ErrorCodes;
}
/**
 * The DTO representing a monetary value, e.g. amount with a currency.
 * The amount of money, in the smallest currency unit (e.g. grosz, cent).
 * The (three letter) currency name, e.g. PLN, USD.
 */
export interface MoneyDTO {
    /**
     * The amount of money, in the smallest currency unit (e.g. grosz, cent).
     */
    Value: number;
    /**
     * The (three letter) currency name, e.g. PLN, USD.
     */
    Currency: string;
}
export interface IReservationRelated {
    ReservationId: string;
}
export interface IWhenOwnsReservation {
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 * @attribute ExampleApp.Examples.Contracts.Booking.Reservations.Authorization.AuthorizeWhenOwnsReservationAttribute
 */
export interface CancelReservation extends Command, IReservationRelated {
}
export namespace CancelReservation {
    export const ErrorCodes = {
        ReservationIdIsInvalid: 1,
        ReservationDoesNotExist: 2,
        ReservationCannotBeCancelled: 3
    } as const;
    export type ErrorCodes = typeof ErrorCodes;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 * @attribute ExampleApp.Examples.Contracts.Booking.Reservations.Authorization.AuthorizeWhenOwnsReservationAttribute
 */
export interface MyReservationById extends Query<MyReservationDTO>, IReservationRelated {
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface MyReservationByTimeslotId extends Query<MyReservationDTO> {
    TimeslotId: string;
}
export interface MyReservationDTO {
    Id: string;
    TimeslotId: string;
    ServiceProviderId: string;
    ServiceProviderName: string;
    Type: ServiceProviderTypeDTO;
    Address: string;
    Location: LocationDTO;
    Date: ApiDateOnly;
    StartTime: string;
    EndTime: string;
    Price: MoneyDTO;
    Status: ReservationStatusDTO;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface MyReservations extends PaginatedQuery<MyReservationDTO> {
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface ReserveTimeslot extends Command {
    CalendarDayId: string;
    TimeslotId: string;
}
export namespace ReserveTimeslot {
    export const ErrorCodes = {
        TimeslotIdInvalid: 1,
        TimeslotCannotBeReserved: 2,
        CalendarDayIdInvalid: 3,
        CalendarDayDoesNotExist: 4
    } as const;
    export type ErrorCodes = typeof ErrorCodes;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface AllServiceProviders extends SortedQuery<ServiceProviderSummaryDTO, ServiceProviderSortFieldsDTO> {
    NameFilter?: string | null;
    TypeFilter?: ServiceProviderTypeDTO | null;
    PromotedOnly: boolean;
}
/**
 * The query will return details about service provider and all available timeslots from
 * to
 *     +X days (configurable on query level).
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface ServiceProviderDetails extends Query<ServiceProviderDetailsDTO> {
    ServiceProviderId: string;
    CalendarDate: ApiDateOnly;
}
export interface ServiceProviderDetailsDTO {
    Id: string;
    Name: string;
    Description: string;
    Type: ServiceProviderTypeDTO;
    Address: string;
    Location: LocationDTO;
    IsPromotionActive: boolean;
    Ratings: number;
    CoverPhoto: string;
    Thumbnail: string;
    Timeslots: TimeslotDTO[];
}
export interface ServiceProviderSummaryDTO {
    Id: string;
    Name: string;
    Type: ServiceProviderTypeDTO;
    Thumbnail: string;
    IsPromotionActive: boolean;
    Address: string;
    Location: LocationDTO;
    Ratings: number;
}
export interface TimeslotDTO {
    Id: string;
    CalendarDayId: string;
    StartTime: string;
    EndTime: string;
    Price: MoneyDTO;
    IsReserved: boolean;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface AssignmentEmployerEmbed extends Query<string> {
}
export interface AdminEmployeeDTO {
    Id: string;
    /**
     * @attribute LeanCode.Contracts.Admin.AdminColumn
     * @attribute LeanCode.Contracts.Admin.AdminSortable
     */
    Name: string;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface AllEmployees extends Query<EmployeeDTO[]> {
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface AllEmployeesAdmin extends AdminQuery<AdminEmployeeDTO> {
    /**
     * @attribute LeanCode.Contracts.Admin.AdminFilterFor
     */
    NameFilter?: string | null;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface CreateEmployee extends Command {
    Name: string;
    Email: string;
}
export namespace CreateEmployee {
    export const ErrorCodes = {
        NameCannotBeEmpty: 1,
        NameTooLong: 2,
        EmailInvalid: 3,
        EmailIsNotUnique: 4
    } as const;
    export type ErrorCodes = typeof ErrorCodes;
}
export interface EmployeeDTO {
    Id: string;
    Name: string;
    Email: string;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface AddNotificationToken extends Command {
    Token: string;
}
export namespace AddNotificationToken {
    export const ErrorCodes = {
        TokenCannotBeEmpty: 1
    } as const;
    export type ErrorCodes = typeof ErrorCodes;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface RemoveNotificationToken extends Command {
    Token: string;
}
export namespace RemoveNotificationToken {
    export const ErrorCodes = {
        TokenCannotBeEmpty: 1
    } as const;
    export type ErrorCodes = typeof ErrorCodes;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface SendCustomNotification extends Command {
    Content: string;
    ImageUrl?: string | null;
}
export namespace SendCustomNotification {
    export const ErrorCodes = {
        ContentCannotBeEmpty: 1,
        ImageUrlInvalid: 2
    } as const;
    export type ErrorCodes = typeof ErrorCodes;
}
export interface KratosIdentityDTO {
    Id: string;
    CreatedAt: ApiDateTimeOffset;
    UpdatedAt: ApiDateTimeOffset;
    SchemaId: string;
    Email: string;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface SearchIdentities extends PaginatedQuery<KratosIdentityDTO> {
    SchemaId?: string | null;
    EmailPattern?: string | null;
    GivenNamePattern?: string | null;
    FamilyNamePattern?: string | null;
}
export interface PaginatedQuery<TResult> extends Query<PaginatedResult<TResult>> {
    /**
     * Zero-based.
     */
    PageNumber: number;
    PageSize: number;
}
export namespace PaginatedQuery {
    export const MinPageSize = 1;
    export const MaxPageSize = 100;
}
export interface PaginatedResult<TResult> {
    Items: TResult[];
    TotalCount: number;
}
/**
 * @attribute LeanCode.Contracts.Security.AllowUnauthorizedAttribute
 */
export interface AddAssignmentsToProject extends Command {
    ProjectId: string;
    Assignments: AssignmentWriteDTO[];
}
export namespace AddAssignmentsToProject {
    export const ErrorCodes = {
        ProjectIdNotValid: 1,
        ProjectDoesNotExist: 2,
        AssignmentsCannotBeNull: 3,
        AssignmentsCannotBeEmpty: 4
    } as const;
    export type ErrorCodes = typeof ErrorCodes;
}
export interface AdminProjectDTO {
    Id: string;
    /**
     * @attribute LeanCode.Contracts.Admin.AdminColumn
     * @attribute LeanCode.Contracts.Admin.AdminSortable
     */
    Name: string;
}
/**
 * @attribute LeanCode.Contracts.Security.AllowUnauthorizedAttribute
 */
export interface AllProjects extends Query<ProjectDTO[]> {
    SortByNameDescending: boolean;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface AllProjectsAdmin extends AdminQuery<AdminProjectDTO> {
    /**
     * @attribute LeanCode.Contracts.Admin.AdminFilterFor
     */
    NameFilter?: string | null;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface AssignEmployeeToAssignment extends Command {
    AssignmentId: string;
    EmployeeId: string;
}
export namespace AssignEmployeeToAssignment {
    export const ErrorCodes = {
        AssignmentIdNotValid: 1,
        ProjectWithAssignmentDoesNotExist: 2,
        EmployeeIdNotValid: 3,
        EmployeeDoesNotExist: 4
    } as const;
    export type ErrorCodes = typeof ErrorCodes;
}
export interface AssignmentDTO extends AssignmentWriteDTO {
    Id: string;
    AssignedEmployeeId?: string | null;
}
export interface AssignmentWriteDTO {
    Name: string;
}
/**
 * @attribute LeanCode.Contracts.Security.AllowUnauthorizedAttribute
 */
export interface CreateProject extends Command {
    Name: string;
}
export namespace CreateProject {
    export const ErrorCodes = {
        NameCannotBeEmpty: 1,
        NameTooLong: 2
    } as const;
    export type ErrorCodes = typeof ErrorCodes;
}
export interface EmployeeAssignedToAssignmentDTO {
    AssignmentId: string;
    EmployeeId: string;
}
export interface EmployeeAssignedToProjectAssignmentDTO {
    ProjectId: string;
    AssignmentId: string;
}
/**
 * @attribute LeanCode.Contracts.Security.AllowUnauthorizedAttribute
 */
export interface EmployeeAssignmentsTopic extends Topic {
    EmployeeId: string;
}
export interface EmployeeUnassignedFromAssignmentDTO {
    AssignmentId: string;
}
export interface EmployeeUnassignedFromProjectAssignmentDTO {
    ProjectId: string;
    AssignmentId: string;
}
export interface ProjectDTO {
    Id: string;
    Name: string;
}
/**
 * @attribute LeanCode.Contracts.Security.AllowUnauthorizedAttribute
 */
export interface ProjectDetails extends Query<ProjectDetailsDTO> {
    Id: string;
}
export interface ProjectDetailsDTO extends ProjectDTO {
    Assignments: AssignmentDTO[];
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface ProjectEmployeesAssignmentsTopic extends Topic {
    ProjectId: string;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface UnassignEmployeeFromAssignment extends Command {
    AssignmentId: string;
}
export namespace UnassignEmployeeFromAssignment {
    export const ErrorCodes = {
        AssignmentIdNotValid: 1,
        ProjectWithAssignmentDoesNotExist: 2
    } as const;
    export type ErrorCodes = typeof ErrorCodes;
}
export interface SortedQuery<TResult, TSort> extends PaginatedQuery<TResult> {
    SortBy: TSort;
    SortByDescending: boolean;
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface DeleteOwnAccount extends Command {
}
export interface Permissions {
}
export namespace Permissions {
    export const RateApp = "RateApp";
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface RatingAlreadySent extends Query<boolean> {
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface SubmitAppRating extends Command {
    Rating: number;
    AdditionalComment?: string | null;
    Platform: PlatformDTO;
    SystemVersion: string;
    AppVersion: string;
    Metadata?: Record<string, Partial<Record<string, any>>> | null;
}
export namespace SubmitAppRating {
    export const ErrorCodes = {
        RatingInvalid: 1,
        AdditionalCommentTooLong: 2,
        PlatformInvalid: 3,
        SystemVersionRequired: 4,
        SystemVersionTooLong: 5,
        AppVersionRequired: 6,
        AppVersionTooLong: 7
    } as const;
    export type ErrorCodes = typeof ErrorCodes;
}
export interface AdminFilterRange<T> {
    From?: T | null;
    To?: T | null;
}
export interface AdminQuery<TResult> extends Query<AdminQueryResult<TResult>> {
    /**
     * 0-based
     */
    Page: number;
    PageSize: number;
    SortDescending?: boolean | null;
    SortBy?: string | null;
}
export interface AdminQueryResult<TResult> {
    Total: number;
    Items: TResult[];
}
export interface ForceUpdateVersionSupport extends Query<ForceUpdateVersionSupportDTO> {
    Platform: ForceUpdatePlatformDTO;
    Version: string;
}
export interface ForceUpdateVersionSupportDTO {
    CurrentlySupportedVersion: string;
    MinimumRequiredVersion: string;
    Result: ForceUpdateVersionSupportResultDTO;
}
export enum ReservationStatusDTO {
    Pending = 0,
    Confirmed = 1,
    Rejected = 2,
    Paid = 3,
    Cancelled = 4,
    Completed = 5
}
export enum ServiceProviderTypeDTO {
    Hairdresser = 0,
    BarberShop = 1,
    Groomer = 2
}
export enum ServiceProviderSortFieldsDTO {
    Name = 0,
    Type = 1,
    Ratings = 2
}
export enum PlatformDTO {
    Android = 0,
    IOS = 1
}
export enum ForceUpdatePlatformDTO {
    Android = 0,
    IOS = 1
}
export enum ForceUpdateVersionSupportResultDTO {
    UpdateRequired = 0,
    UpdateSuggested = 1,
    UpToDate = 2
}
export default function (cqrsClient: CQRS) {
    return {
        AddTimeslot: cqrsClient.createCommand<AddTimeslot, AddTimeslot.ErrorCodes>("ExampleApp.Examples.Contracts.Booking.Management.AddTimeslot", AddTimeslot.ErrorCodes),
        CreateServiceProvider: cqrsClient.createCommand<CreateServiceProvider, CreateServiceProvider.ErrorCodes>("ExampleApp.Examples.Contracts.Booking.Management.CreateServiceProvider", CreateServiceProvider.ErrorCodes),
        CancelReservation: cqrsClient.createCommand<CancelReservation, CancelReservation.ErrorCodes>("ExampleApp.Examples.Contracts.Booking.Reservations.CancelReservation", CancelReservation.ErrorCodes),
        MyReservationById: cqrsClient.createQuery<MyReservationById, MyReservationDTO | null | undefined>("ExampleApp.Examples.Contracts.Booking.Reservations.MyReservationById"),
        MyReservationByTimeslotId: cqrsClient.createQuery<MyReservationByTimeslotId, MyReservationDTO | null | undefined>("ExampleApp.Examples.Contracts.Booking.Reservations.MyReservationByTimeslotId"),
        MyReservations: cqrsClient.createQuery<MyReservations, PaginatedResult<MyReservationDTO>>("ExampleApp.Examples.Contracts.Booking.Reservations.MyReservations"),
        ReserveTimeslot: cqrsClient.createCommand<ReserveTimeslot, ReserveTimeslot.ErrorCodes>("ExampleApp.Examples.Contracts.Booking.Reservations.ReserveTimeslot", ReserveTimeslot.ErrorCodes),
        AllServiceProviders: cqrsClient.createQuery<AllServiceProviders, PaginatedResult<ServiceProviderSummaryDTO>>("ExampleApp.Examples.Contracts.Booking.ServiceProviders.AllServiceProviders"),
        ServiceProviderDetails: cqrsClient.createQuery<ServiceProviderDetails, ServiceProviderDetailsDTO | null | undefined>("ExampleApp.Examples.Contracts.Booking.ServiceProviders.ServiceProviderDetails"),
        AssignmentEmployerEmbed: cqrsClient.createQuery<AssignmentEmployerEmbed, string>("ExampleApp.Examples.Contracts.Dashboards.AssignmentEmployerEmbed"),
        AllEmployees: cqrsClient.createQuery<AllEmployees, EmployeeDTO[]>("ExampleApp.Examples.Contracts.Employees.AllEmployees"),
        AllEmployeesAdmin: cqrsClient.createQuery<AllEmployeesAdmin, AdminQueryResult<AdminEmployeeDTO>>("ExampleApp.Examples.Contracts.Employees.AllEmployeesAdmin"),
        CreateEmployee: cqrsClient.createCommand<CreateEmployee, CreateEmployee.ErrorCodes>("ExampleApp.Examples.Contracts.Employees.CreateEmployee", CreateEmployee.ErrorCodes),
        AddNotificationToken: cqrsClient.createCommand<AddNotificationToken, AddNotificationToken.ErrorCodes>("ExampleApp.Examples.Contracts.Firebase.AddNotificationToken", AddNotificationToken.ErrorCodes),
        RemoveNotificationToken: cqrsClient.createCommand<RemoveNotificationToken, RemoveNotificationToken.ErrorCodes>("ExampleApp.Examples.Contracts.Firebase.RemoveNotificationToken", RemoveNotificationToken.ErrorCodes),
        SendCustomNotification: cqrsClient.createCommand<SendCustomNotification, SendCustomNotification.ErrorCodes>("ExampleApp.Examples.Contracts.Firebase.SendCustomNotification", SendCustomNotification.ErrorCodes),
        SearchIdentities: cqrsClient.createQuery<SearchIdentities, PaginatedResult<KratosIdentityDTO>>("ExampleApp.Examples.Contracts.Identities.SearchIdentities"),
        AddAssignmentsToProject: cqrsClient.createCommand<AddAssignmentsToProject, AddAssignmentsToProject.ErrorCodes>("ExampleApp.Examples.Contracts.Projects.AddAssignmentsToProject", AddAssignmentsToProject.ErrorCodes),
        AllProjects: cqrsClient.createQuery<AllProjects, ProjectDTO[]>("ExampleApp.Examples.Contracts.Projects.AllProjects"),
        AllProjectsAdmin: cqrsClient.createQuery<AllProjectsAdmin, AdminQueryResult<AdminProjectDTO>>("ExampleApp.Examples.Contracts.Projects.AllProjectsAdmin"),
        AssignEmployeeToAssignment: cqrsClient.createCommand<AssignEmployeeToAssignment, AssignEmployeeToAssignment.ErrorCodes>("ExampleApp.Examples.Contracts.Projects.AssignEmployeeToAssignment", AssignEmployeeToAssignment.ErrorCodes),
        CreateProject: cqrsClient.createCommand<CreateProject, CreateProject.ErrorCodes>("ExampleApp.Examples.Contracts.Projects.CreateProject", CreateProject.ErrorCodes),
        EmployeeAssignmentsTopic: cqrsClient.createTopic<EmployeeAssignmentsTopic, {
            "ExampleApp.Examples.Contracts.Projects.EmployeeAssignedToProjectAssignmentDTO": EmployeeAssignedToProjectAssignmentDTO;
            "ExampleApp.Examples.Contracts.Projects.EmployeeUnassignedFromProjectAssignmentDTO": EmployeeUnassignedFromProjectAssignmentDTO;
        }>("ExampleApp.Examples.Contracts.Projects.EmployeeAssignmentsTopic"),
        ProjectDetails: cqrsClient.createQuery<ProjectDetails, ProjectDetailsDTO | null | undefined>("ExampleApp.Examples.Contracts.Projects.ProjectDetails"),
        ProjectEmployeesAssignmentsTopic: cqrsClient.createTopic<ProjectEmployeesAssignmentsTopic, {
            "ExampleApp.Examples.Contracts.Projects.EmployeeAssignedToAssignmentDTO": EmployeeAssignedToAssignmentDTO;
            "ExampleApp.Examples.Contracts.Projects.EmployeeUnassignedFromAssignmentDTO": EmployeeUnassignedFromAssignmentDTO;
        }>("ExampleApp.Examples.Contracts.Projects.ProjectEmployeesAssignmentsTopic"),
        UnassignEmployeeFromAssignment: cqrsClient.createCommand<UnassignEmployeeFromAssignment, UnassignEmployeeFromAssignment.ErrorCodes>("ExampleApp.Examples.Contracts.Projects.UnassignEmployeeFromAssignment", UnassignEmployeeFromAssignment.ErrorCodes),
        DeleteOwnAccount: cqrsClient.createCommand<DeleteOwnAccount, {}>("ExampleApp.Examples.Contracts.Users.DeleteOwnAccount", {}),
        RatingAlreadySent: cqrsClient.createQuery<RatingAlreadySent, boolean>("LeanCode.AppRating.Contracts.RatingAlreadySent"),
        SubmitAppRating: cqrsClient.createCommand<SubmitAppRating, SubmitAppRating.ErrorCodes>("LeanCode.AppRating.Contracts.SubmitAppRating", SubmitAppRating.ErrorCodes),
        ForceUpdateVersionSupport: cqrsClient.createQuery<ForceUpdateVersionSupport, ForceUpdateVersionSupportDTO>("LeanCode.ForceUpdate.Contracts.VersionSupport")
    };
}
