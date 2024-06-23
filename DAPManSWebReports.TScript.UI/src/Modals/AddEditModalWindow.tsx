import './Modal.css';
import React, { useState, useEffect } from 'react';
import { Modal, Form, Button, ButtonToolbar } from 'rsuite';


const AddEditReportModal: React.FC<{ show: boolean, report: any, onSave: (report: any) => void, onClose: () => void }> = ({ show, report, onSave, onClose }) => {
    const [formValue, setFormValue] = useState<any>(report || {
        name: '',
        author: '',
        dateCreated: new Date().toLocaleDateString(),
        dataSource: '',
        sqlQuery: ''
    });

    useEffect(() => {
        setFormValue(report || {
          name: '',
          author: '',
          dateCreated: new Date().toLocaleDateString(),
          dataSource: '',
          sqlQuery: ''
        });
      }, [report]);

      const handleChange = (value: any, name: string) => {
        setFormValue({...formValue, [name]: value});
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
              <Form.Group>
                <Form.ControlLabel>Наименование</Form.ControlLabel>
                <Form.Control name="name" value={formValue.name} onChange={handleChange} />
              </Form.Group>
              <Form.Group>
                <Form.ControlLabel>ИД источника данных</Form.ControlLabel>
                <Form.Control name="datasourceId" value={formValue.dataSourceID} onChange={handleChange} />
              </Form.Group>
              <Form.Group>
                <Form.ControlLabel>Описание</Form.ControlLabel>
                <Form.Control name="dataviewnote" value={formValue.dataviewNote} onChange={handleChange} />
              </Form.Group>
              <Form.Group>
                <Form.ControlLabel>SQL Запрос</Form.ControlLabel>
                <Form.Control
                  name="sqlQuery"
                  componentClass="textarea"
                  rows={5}
                  value={formValue.query}
                  onChange={handleChange}
                />
              </Form.Group>
            </Form>
          </Modal.Body>
          <Modal.Footer>
            <ButtonToolbar>
              <Button onClick={handleSubmit} appearance="primary">
                Сохранить
              </Button>
              <Button onClick={onClose} appearance="subtle">
                Отмена
              </Button>
            </ButtonToolbar>
          </Modal.Footer>
        </Modal>
      );
    };
export default AddEditReportModal;