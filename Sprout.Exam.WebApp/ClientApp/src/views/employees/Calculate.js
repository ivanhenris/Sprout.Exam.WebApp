import React, { Component } from 'react';
import authService from '../../components/api-authorization/AuthorizeService';

export class EmployeeCalculate extends Component {
  static displayName = EmployeeCalculate.name;

  constructor(props) {
    super(props);
    this.state = { id: 0,fullName: '',birthdate: '',tin: '',typeId: 1,period: 0,netIncome: 0, loading: true,loadingCalculate:false, label: '', payLabel: '', tax: 0, basePay: 0, employeeData: [], errors: []};
  }

  async componentDidMount() {
    await this.getEmployee(this.props.match.params.id);
    await this.getEmployeeTypeLabel(this.state.typeId);
  }
  handleChange(event) {
    this.setState({ [event.target.name] : event.target.value});
    this.setState({ errors: [] });
  }

  async handleChangeLabel(event) {
    await this.setState({ [event.target.name] : event.target.value});
    await this.getEmployeeTypeLabel(this.state.typeId);
  }

  roundToTwo(num) {
    return +(Math.round(num + "e+2")  + "e-2");
}

  handleSubmit(e){
      const value = this.state.period;
      let periodCheck = !isNaN(+value);
      if (this.state.period === '') {
        e.preventDefault();
        console.log(this.state.period)
        this.fieldHighlighting();
        alert('Please fill all required fields.');
        return;
      }
      else if(periodCheck === false){
        e.preventDefault();
        alert('Pay input is invalid');
        return;
      }
      e.preventDefault();
      this.calculateSalary();
  }

  fieldHighlighting() {
    let errors = [];
    let {period} = this.state;
    if(!period) {
      errors.push('period');
    }
    if (errors.length) { 
      this.setState({ errors });
      return;
    }
  }

  render() {

    let contents = this.state.loading
    ? <p><em>Loading...</em></p>
    : <div>
    <form>
<div className='form-row'>
<div className='form-group col-md-12'>
  <label>Full Name: <b>{this.state.fullName}</b></label>
</div>

</div>

<div className='form-row'>
<div className='form-group col-md-12'>
  <label >Birthdate: <b>{new Date(this.state.birthdate).toDateString().slice(4,15)}</b></label>
</div>
</div>

<div className="form-row">
<div className='form-group col-md-12'>
  <label>TIN: <b>{this.state.tin}</b></label>
</div>
</div>

<div className="form-row">
<div className='form-group col-md-12'>
  <label>Employee Type: <b>{this.state.typeId === 1?"Regular": "Contractual"}</b></label>
</div>
</div>


<div className="form-row">
<div className='form-group col-md-12'>
  <label>{this.state.payLabel}: <b>{this.state.basePay}</b></label>
</div>
</div>

{ this.state.tax ? 
  <div className="form-row">
<div className='form-group col-md-12'>
  <label>Tax: <b>{this.state.tax}%</b></label>
</div>
</div>
: null}

<div className="form-row">
<div className='form-group col-md-6'>
  <label htmlFor='inputPeriod4'>{this.state.label}: *</label>
  <input type='text' className='form-control' id='inputPeriod4' onChange={this.handleChange.bind(this)} value={this.state.period} name="period" placeholder='0'  style={{ borderColor: this.state.errors.includes('period') ? 'red' : 'LightGray' }}/>
</div>
</div>

<div className="form-row">
<div className='form-group col-md-12'>
  <label>Net Income: <b>{this.roundToTwo(this.state.netIncome)}</b></label>
</div>
</div>

<button type="submit" onClick={this.handleSubmit.bind(this)} disabled={this.state.loadingCalculate} className="btn btn-primary mr-2">{this.state.loadingCalculate?"Loading...": "Calculate"}</button>
<button type="button" onClick={() => this.props.history.push("/employees/index")} className="btn btn-primary">Back</button>
</form>
</div>;


    return (
        <div>
        <h1 id="tabelLabel" >Employee Calculate Salary</h1>
        <br/>
        {contents}
      </div>
    );
  }

  async calculateSalary() {
    this.setState({ loadingCalculate: true });
    const token = await authService.getAccessToken();
    const requestOptions = {
        method: 'POST',
        headers: !token ? {} : { 'Authorization': `Bearer ${token}`,'Content-Type': 'application/json' },
        body: JSON.stringify(this.state)
    };
    const response = await fetch('api/employees/' + this.state.id + '/calculate',requestOptions);
    if(response.status === 500) {
      alert("There was an error occured.");
    }
    else if(response.status === 404) {
      alert("Employee Type not found.");
    }
    else if(response.status === 200){
      const data = await response.json();
      this.setState({ loadingCalculate: false,netIncome: data });
    }
  }

  async getEmployee(id) {
    this.setState({ loading: true,loadingCalculate: false });
    const token = await authService.getAccessToken();
    const response = await fetch('api/employees/' + id, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    if(response.status === 200){
        const data = await response.json();
        this.setState({ id: data.id,fullName: data.fullName,birthdate: data.birthdate,tin: data.tin,typeId: data.typeId, loading: false,loadingCalculate: false, basePay: data.basePay });
    }
    else{
        alert("There was an error occured.");
        this.setState({ loading: false,loadingCalculate: false });
    }
  }

  async getEmployeeTypeLabel(id) {
    this.setState({ loading: true,loadingSave: false });
    const token = await authService.getAccessToken();
    const response = await fetch('api/employeetype/' + id, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    console.log(response.status)
    if(response.status === 500)
    {
      alert("There was an error occured.");
    }
    this.setState({ label: data.dayLabel, payLabel: data.payLabel, tax: data.tax, loading: false, employeeType: data });
  }
}
