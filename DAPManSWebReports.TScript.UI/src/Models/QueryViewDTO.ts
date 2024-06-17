export interface QueryViewDTO {
    id           : number;
    name         : string;
    dataSourceId : number;
    title        : string;
    result: Array<{ [key: string]: any }>;
    totalCount: number;
    offset: number;
    pageSize: number;
    queryResult:string;
    pagedItems: Array<{ [key: string]: any }>;
}