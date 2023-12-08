/*eslint-disable import/no-anonymous-default-export, unused-imports/no-unused-vars-ts, @typescript-eslint/no-unused-vars, @typescript-eslint/ban-types, @typescript-eslint/no-empty-interface, @typescript-eslint/no-namespace, @nx/enforce-module-boundaries, @typescript-eslint/no-explicit-any*/
import type { ApiDateTimeOffset } from "@leancodepl/api-date-datefns"
import type { ReactQueryCqrs as CQRS } from ".";

export type Query<TResult> = {}
export type Command = {}
export type Operation<TResult> = {}
export type Topic = {}

export interface Auth {
}
export interface Clients {
}
export namespace Clients {
    export const AdminApp = "admin_app";
    export const ClientApp = "client_app";
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
export interface Scopes {
}
export namespace Scopes {
    export const InternalApi = "internal_api";
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface AssignmentEmployerEmbed extends Query<string> {
}
/**
 * @attribute LeanCode.Contracts.Security.AuthorizeWhenHasAnyOfAttribute
 */
export interface AllEmployees extends Query<EmployeeDTO[]> {
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
        AssignmentEmployerEmbed: cqrsClient.createQuery<AssignmentEmployerEmbed, string>("ExampleApp.Examples.Contracts.Dashboards.AssignmentEmployerEmbed"),
        AllEmployees: cqrsClient.createQuery<AllEmployees, EmployeeDTO[]>("ExampleApp.Examples.Contracts.Employees.AllEmployees"),
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
