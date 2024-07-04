import React, { useState } from 'react';
import { Button          } from 'rsuite';
import config              from '../../Utils/config';
import tokenService from '../../Services/tokenService';

interface Props 
{
    dataviewid: number | null;  
    queryparams: ViewParams;
    title: string;
}

interface ViewParams 
{
    startDate: string;
    stopDate: string;
    presetDate: string;
    format: string;
    sortOrder: string;
    sortColumnNumber: number;
}

const ExcelDataComponent: React.FC<Props> = ({ dataviewid, queryparams, title }) => {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    
    const fetchdata = async (dataviewid: number | null, viewParams: ViewParams) =>
    {
        if (dataviewid === null) return;
        try
        {
            setIsLoading(true);
            const params = new URLSearchParams(
                {
                    startDate: viewParams.startDate,
                    stopDate: viewParams.stopDate,
                    presetDate: viewParams.presetDate,
                    format: viewParams.format,
                    sortOrder: viewParams.sortOrder,
                    sortColumnNumber: viewParams.sortColumnNumber.toString(),
                    export: 'true',
                    
                });
            const headers = tokenService.getAuthHeaders();
            const response = await fetch(`${config.ApiBaseUrlDev}/exceldata/${dataviewid}?${params.toString()}`,{headers:headers});
            if (!response.ok) {
                throw new Error(`Error: ${response.status}`);
            }
            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            const fileName = title + 
            (viewParams.startDate && viewParams.stopDate ? 
                ` с ${viewParams.startDate} по ${viewParams.stopDate}` : 
                '');

            link.setAttribute('download', `${fileName}.xlsx`);
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
        catch (error: any)
        {
            setError(error.message);
        }
        finally
        {
            setIsLoading(false);
        }
    };

    const handleGenerateExcelFile = () => 
        {
            if (dataviewid !== null) 
                {
                    fetchdata(dataviewid, queryparams);
                }
        };
        
    return (
        <div style={{ marginLeft: 50, textAlign: 'center' }} >
            {error && <div style={{ color: 'red' }}>{error}</div>}
            <Button appearance="primary" onClick={handleGenerateExcelFile} className='custom-button' loading={isLoading} style={{width:'150px'}}>
                {isLoading ? 'Загружаем данные...' : 'Скачать отчет'}
          </Button>
      </div>
    );
};

export default ExcelDataComponent;