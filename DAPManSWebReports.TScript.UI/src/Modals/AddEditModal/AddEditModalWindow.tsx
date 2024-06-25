import '../Modal.css';
import React, { useState, useEffect } from 'react';
import { Modal, Form, Button, ButtonToolbar, Input, Tabs, FlexboxGrid } from 'rsuite';
import { DataViewDTO } from '../../Models/DataViewDTO';
import config from '../../Utils/config';

interface AddEitReportModalProps {
  show: boolean;
  report: DataViewDTO | null;
  onSave: (updatedReport: DataViewDTO) => void;
  onClose: () => void;
}

//@ts-ignore
const AddEditReportModal: React.FC<AddEitReportModalProps> = ({ show, report, onSave, onClose }) => {
    const [formValue, setFormValue] = useState<DataViewDTO>(report || {
      id:             0,
      name:           '',
      parentid:       0,
      dataviewNote:   '',
      startDateField: '',
      endDateField:   '',
      dataSourceID:   0,
      reportType:     '',
      query:          '',
      folderid:       0,
      remotePassword: '',
      remoteUser:     '',
      reportFormat:   0
    });

    useEffect(() => {
      if (report) {
        setFormValue({
          name:           report?.name           || '',
          dataviewNote:   report?.dataviewNote   || '',
          dataSourceID:   report?.dataSourceID   || 0,
          query:          report?.query          || '',
          folderid:       report?.folderid       || 0,
          id:             report?.id             || 0,
          parentid:       report?.parentid       || 0,
          startDateField: report?.startDateField || '',
          endDateField:   report?.endDateField   || '',
          reportType:     report?.reportType     || '',
          remotePassword: report?.remotePassword || '',
          remoteUser:     report?.remoteUser     || '',
          reportFormat:   report?.reportFormat   || 0
        })}
    }, [report]);

const handleChange = (value: any, name: string) => {
    setFormValue(prevState => ({
      ...prevState,
      [name]: value
    }));
  };

const handleSave = async () => {
  if (formValue) {
      try {
          const response = await fetch(`${config.ApiBaseUrlDev}/dataview/${formValue.id}`, {
              method: 'PUT',
              headers: {
                  'Content-Type': 'application/json'
              },
              body: JSON.stringify(formValue)
          });

          if (response.status === 400) {
              const errorData = await response.json();
              console.error('Validation error', errorData);
          } else if (response.ok) {
              onSave(formValue);
              onClose();
          } else {
              console.error('Failed to save', response.statusText);
          }
      } catch (error) {
          console.error('Error saving report data', error);
      }
  }
};

    const handleSubmit = () => {
        onSave(formValue);
    };

    return (
        <Modal open={show} onClose={onClose}>
          <Modal.Header>
            <Modal.Title>{report ? 'Редактировать отчет' : 'Добавить отчет'}</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Form fluid>
            <Tabs defaultActiveKey="1" appearance="subtle" className='tabStyle'>
            <Tabs.Tab eventKey="1" title="Общая информация" >
            <Form.Group>
              <Form.ControlLabel>ID Представления</Form.ControlLabel>
              <Form.Control
                name="id"
                type="number"
                value={formValue.id}
                onChange={(value) => handleChange(value, 'id')}
              />
           </Form.Group>
           <Form.Group>
              <Form.ControlLabel>Наименование</Form.ControlLabel>
              <Form.Control
                name="name"
                type="string"
                value={formValue.name}
                onChange={(value: string) => handleChange(value, 'name')}
              />
          </Form.Group>
          <Form.Group>
            <Form.ControlLabel>ИД источника данных</Form.ControlLabel>
            <Form.Control
              name="dataSourceID"
              type="number"
              value={formValue.dataSourceID}
              onChange={(value) => handleChange(value, 'dataSourceID')}
            />
          </Form.Group>
          <Form.Group>
            <Form.ControlLabel>Описание</Form.ControlLabel>
            <Input
              as="textarea"
              name="dataviewNote"
              value={formValue.dataviewNote}
              onChange={(value) => handleChange(value, 'dataviewNote')}
            />
          </Form.Group>
          <Form.Group>
            <Form.ControlLabel>Родительский каталог</Form.ControlLabel>
            <Form.Control
              name="folderid"
              type="number"
              value={formValue.folderid}
              onChange={(value) => handleChange(value, 'folderid')}
            />
          </Form.Group>
          </Tabs.Tab>
          <Tabs.Tab eventKey="2" title="Информация о запросе">
          <Form.Group>
            <Form.ControlLabel style={{textAlign:'center'}}>Параметры даты:</Form.ControlLabel> 
            <Form.Group >
            <Form.ControlLabel>Дата начала</Form.ControlLabel> 
            <Input
              name="startDateField"
              value={formValue.startDateField}
              onChange={(value) => handleChange(value, 'startDateField')}
            />
            <Form.ControlLabel>Дата окончания</Form.ControlLabel> 
            <Input
              name="endDateField"
              value={formValue.endDateField}
              onChange={(value) => handleChange(value, 'endDateField')}
            />
            </Form.Group>
            <Form.ControlLabel style={{textAlign:'center'}}>Текст запроса:</Form.ControlLabel>
            <Input
              as="textarea"
              name="query"
              value={formValue.query}
              onChange={(value) => handleChange(value, 'query')}
        />
          </Form.Group>
          </Tabs.Tab>
          </Tabs>
            </Form>
          </Modal.Body>
          <Modal.Footer>
          <FlexboxGrid justify="space-between">
          <FlexboxGrid.Item>
            <ButtonToolbar>
              <Button onClick={handleSave} appearance="primary">
                Сохранить
              </Button>
              </ButtonToolbar>
              </FlexboxGrid.Item>
              <FlexboxGrid.Item>
              <ButtonToolbar>
              <Button onClick={onClose} appearance="subtle">
                Отмена
              </Button>
            </ButtonToolbar>
            </FlexboxGrid.Item>
            </FlexboxGrid>
          </Modal.Footer>
        </Modal>
      );
    };
export default AddEditReportModal;