export interface QueryViewDTO {
    id           : number;
    name         : string;
    DataSourceId : number;
    title        : string;
    result: Array<{ [key: string]: any }>;
    totalCount: number;
    offset: number;
    pageSize: number;
}