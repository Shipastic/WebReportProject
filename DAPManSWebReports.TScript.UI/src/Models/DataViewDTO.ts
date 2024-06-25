export interface DataViewDTO {
    id               : number;
    name             : string;
    parentid         : number;
    dataviewNote     : string;
    startDateField   : string;
    endDateField     : string;
    reportType       : string;
    query            : string;
    folderid         : number;
    dataSourceID     : number;
    remotePassword   : string;
    remoteUser       : string;
    reportFormat     : number;
}
